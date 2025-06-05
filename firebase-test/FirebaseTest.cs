using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;

namespace FirebaseTest
{
    class Program
    {
        private static readonly string ApiKey = "AIzaSyDKAfhQ9rYxk-_AYZeYoEjyiN4CuibRhYo";
        private static readonly string AuthDomain = "taghunt-2507b.firebaseapp.com";
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Firebase Authentication Test ===");
            
            try
            {
                // Initialize Firebase Auth
                var client = new FirebaseAuthClient(new FirebaseAuthConfig
                {
                    ApiKey = ApiKey,
                    AuthDomain = AuthDomain,
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider()
                    }
                });
                
                Console.WriteLine("Firebase Auth client initialized successfully");
                
                // Test 1: Try to register a new user
                Console.WriteLine("\n--- Test 1: User Registration ---");
                string testEmail = "svedmanp@gmail.com";
                string testPassword = "p0nTus5v";
                
                try
                {
                    var userCredential = await client.CreateUserWithEmailAndPasswordAsync(testEmail, testPassword);
                    Console.WriteLine($"✅ Registration successful! User ID: {userCredential.User.Uid}");
                    
                    // Test 2: Try to login with the same user
                    Console.WriteLine("\n--- Test 2: User Login ---");
                    var loginCredential = await client.SignInWithEmailAndPasswordAsync(testEmail, testPassword);
                    Console.WriteLine($"✅ Login successful! User ID: {loginCredential.User.Uid}");
                    Console.WriteLine($"   Email: {loginCredential.User.Info?.Email}");
                    
                    Console.WriteLine("\n🎉 All tests passed! Firebase Authentication is working correctly.");
                }
                catch (Exception regEx)
                {
                    Console.WriteLine($"❌ Registration failed: {regEx.Message}");
                    
                    // If registration failed because user exists, try login
                    if (regEx.Message.Contains("EMAIL_EXISTS"))
                    {
                        Console.WriteLine("User already exists, trying login...");
                        try
                        {
                            var loginCredential = await client.SignInWithEmailAndPasswordAsync(testEmail, testPassword);
                            Console.WriteLine($"✅ Login successful! User ID: {loginCredential.User.Uid}");
                            Console.WriteLine($"   Email: {loginCredential.User.Info?.Email}");
                            Console.WriteLine("\n🎉 Firebase Authentication is working correctly.");
                        }
                        catch (Exception loginEx)
                        {
                            Console.WriteLine($"❌ Login also failed: {loginEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Firebase initialization failed: {ex.Message}");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
} 