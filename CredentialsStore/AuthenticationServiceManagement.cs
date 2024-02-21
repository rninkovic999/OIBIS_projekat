using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Config;
using Common.Cryptography;

// LOGOVANJE 

namespace CredentialsStore
{
    public class AuthenticationServiceManagement : IAuthenticationServiceManagement
    {
        UsersDB db = new UsersDB();

        ConfigurationManager config = new ConfigurationManager();

        Dictionary<string, int> failedAttempts = new Dictionary<string, int>();

        public int ValidateCredentials(byte[] username, byte[] password, byte[] signature)
        {
            // -3  DISABLED
            // -2  LOCKED
            // -1  USER DATA DOES NOT EXIST
            // 0   USER DOES NOT EXISTS
            // 1   IF USER DATA IS VALID

            List<User> users = db.getUsers();

            byte[] data = new byte[username.Length + password.Length];
            Buffer.BlockCopy(username, 0, data, 0, username.Length);
            Buffer.BlockCopy(password, 0, data, username.Length, password.Length);


            if (DigitalSignatureHelperFunctions.VerifyDigitalSignature(data, signature))
            {
                string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));
                string outPassword = AES.DecryptData(password, SecretKey.LoadKey(AES.KeyLocation));

                for (int i = 0; i < users.Count(); i++)
                    if (users[i].GetUsername() == outUsername && (users[i].GetPassword() == outPassword.GetHashCode().ToString()))
                    {
                        if (users[i].GetDisabled())
                        {
                            db.addUsers(users);
                            return -3; // DISABLED
                        }
                        if (users[i].GetLocked())
                        {
                            db.addUsers(users);
                            return -2; // LOCKED
                        }

                        Console.WriteLine($"Account - {outUsername} with password {outPassword} verified successfully.\n");
                        users[i].SetLoggedTime(); 
                        db.addUsers(users);
                        return 1;
                    }

                    
                    else if (users[i].GetUsername() == outUsername && (users[i].GetPassword() != outPassword.GetHashCode().ToString()))
                    {
                        if (failedAttempts.ContainsKey(outUsername))
                        {
                            failedAttempts[outUsername]++;
                            if (failedAttempts[outUsername] == config.GetFailedAttempts())
                            {
                                failedAttempts.Remove(outUsername);
                                users[i].SetLocked(true);
                                users[i].SetLockedTime();
                                db.addUsers(users);
                                return -2; //LOCKED
                            }

                        }
                        else
                            failedAttempts.Add(outUsername, 1);

                        return 0; //PASSWORD IS NOT VALID
                    }

                return -1; //USER DATA IS NOT VALID
            }
            else
                return -4; //SIGNATURE IS NOT VALID
        }

        //RETURNS  0  IF DATA IS RESET
        //RETURNS -1  IF SIGNATURE IS NOT VALID

        public int ResetUserOnLogOut(byte[] username, byte[] signature)
        {
            List<User> users = db.getUsers();

            if (DigitalSignatureHelperFunctions.VerifyDigitalSignature(username, signature))
            {
                //Decrypting data
                string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));

                for (int i = 0; i < users.Count(); i++)
                    if (users[i].GetUsername() == outUsername)
                    {
                        users[i].SetLoggedTime(string.Empty);
                        db.addUsers(users);
                        break;
                    }
                return 0; //DATA IS RESET
            }
            else
                return -1; //SIGNATURE IS NOT VALID
        }
        // -5 SIGNATURE IS NOT VALID
        // -4 TIMEOUT
        // -3 DISABLED
        // -2 LOCKED
        // -1 IF USER DOES NOT EXISTS
        //  0  IF USER DATA IS VALID

        public int CheckIn(byte[] username, byte[] signature)
        {
            List<User> users = db.getUsers();

            if (DigitalSignatureHelperFunctions.VerifyDigitalSignature(username, signature))
            {
                string outUsername = AES.DecryptData(username, SecretKey.LoadKey(AES.KeyLocation));

                for (int i = 0; i < users.Count(); i++)
                    if (users[i].GetUsername() == outUsername)
                    {
                        if (users[i].GetDisabled())
                        {
                            db.addUsers(users);
                            return -3; //DISABLED
                        }
                        if (users[i].GetLocked())
                        {
                            db.addUsers(users);
                            return -2; //LOCKED
                        }
                        if (users[i].GetLoggedInTime() == "")
                        {
                            db.addUsers(users);
                            return -4; // TIME OUT
                        }
                        Console.WriteLine($"Account - {outUsername} checked in successfully.\n");
                        db.addUsers(users);
                        return 1;
                    }

                return -1;
            }
            else
                return -5; //SIGNATURE IS NOT VALID

        }

    }
}

