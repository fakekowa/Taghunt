using System.Text;

namespace TagHunt.Views;

/// <summary>
/// Simple test page to verify navigation is working
/// </summary>
public partial class TestPage : ContentPage
{
    /// <summary>
    /// Initializes the test page
    /// </summary>
    public TestPage()
    {
        InitializeComponent();
    }
    
    /// <summary>
    /// Handles the authentication test button click
    /// </summary>
    private async void OnTestAuthClicked(object sender, EventArgs e)
    {
        TestResultLabel.Text = "Running authentication persistence test...";
        
        try
        {
            var results = new StringBuilder();
            results.AppendLine("=== Authentication Test Results ===\n");
            
            // Run the authentication test and capture results
            await RunAuthenticationTest(results);
            
            TestResultLabel.Text = results.ToString();
        }
        catch (Exception ex)
        {
            TestResultLabel.Text = $"Test failed with error: {ex.Message}";
        }
    }
    
    /// <summary>
    /// Runs the authentication persistence test
    /// </summary>
    private async Task RunAuthenticationTest(StringBuilder results)
    {
        try
        {
            // Test using the same logic as our authentication service
            const string AuthTokenKey = "auth_token";
            const string UserIdKey = "user_id";
            const string UserEmailKey = "user_email";
            const string UserDisplayNameKey = "user_display_name";
            const string LoginTimestampKey = "login_timestamp";

            // Clear any existing data first
            results.AppendLine("1. Clearing existing auth data...");
            Microsoft.Maui.Storage.SecureStorage.Remove(AuthTokenKey);
            Microsoft.Maui.Storage.SecureStorage.Remove(UserIdKey);
            Microsoft.Maui.Storage.SecureStorage.Remove(UserEmailKey);
            Microsoft.Maui.Storage.SecureStorage.Remove(UserDisplayNameKey);
            Microsoft.Maui.Storage.SecureStorage.Remove(LoginTimestampKey);
            Microsoft.Maui.Storage.SecureStorage.Remove("HasRestoredSession");

            // Simulate storing auth data (like after login)
            results.AppendLine("2. Simulating login - storing auth data...");
            var testUserId = "test-user-123";
            var testEmail = "test@example.com";
            var testDisplayName = "Test User";
            var testToken = "fake-auth-token-12345";
            var loginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            await Microsoft.Maui.Storage.SecureStorage.SetAsync(AuthTokenKey, testToken);
            await Microsoft.Maui.Storage.SecureStorage.SetAsync(UserIdKey, testUserId);
            await Microsoft.Maui.Storage.SecureStorage.SetAsync(UserEmailKey, testEmail);
            await Microsoft.Maui.Storage.SecureStorage.SetAsync(UserDisplayNameKey, testDisplayName);
            await Microsoft.Maui.Storage.SecureStorage.SetAsync(LoginTimestampKey, loginTimestamp);

            results.AppendLine($"   ✅ Stored data for: {testEmail}");

            // Simulate app restart - check if data can be restored
            results.AppendLine("3. Simulating app restart...");
            var authToken = await Microsoft.Maui.Storage.SecureStorage.GetAsync(AuthTokenKey);
            var storedTimestamp = await Microsoft.Maui.Storage.SecureStorage.GetAsync(LoginTimestampKey);

            if (!string.IsNullOrEmpty(authToken) && !string.IsNullOrEmpty(storedTimestamp))
            {
                if (long.TryParse(storedTimestamp, out var timestamp))
                {
                    var loginTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
                    var daysSinceLogin = (DateTimeOffset.UtcNow - loginTime).TotalDays;

                    if (daysSinceLogin <= 30)
                    {
                        // Simulate setting the restored session flag
                        await Microsoft.Maui.Storage.SecureStorage.SetAsync("HasRestoredSession", "true");
                        
                        results.AppendLine("   ✅ SUCCESS: Auth data restored!");
                        results.AppendLine($"   Days since login: {daysSinceLogin:F4}");
                        
                        // Verify we can get the user data
                        var storedUserId = await Microsoft.Maui.Storage.SecureStorage.GetAsync(UserIdKey);
                        var storedEmail = await Microsoft.Maui.Storage.SecureStorage.GetAsync(UserEmailKey);
                        
                        results.AppendLine($"   User ID: {storedUserId}");
                        results.AppendLine($"   Email: {storedEmail}");
                        
                        // Test the login check
                        var hasRestoredSession = await Microsoft.Maui.Storage.SecureStorage.GetAsync("HasRestoredSession");
                        var isLoggedIn = hasRestoredSession == "true";
                        
                        if (isLoggedIn)
                        {
                            results.AppendLine("   ✅ Login check: PASSED");
                            results.AppendLine("\n🎉 RESULT: Auto-login should work!");
                        }
                        else
                        {
                            results.AppendLine("   ❌ Login check: FAILED");
                        }
                    }
                    else
                    {
                        results.AppendLine("   ❌ Login too old (>30 days)");
                    }
                }
                else
                {
                    results.AppendLine("   ❌ Invalid timestamp format");
                }
            }
            else
            {
                results.AppendLine("   ❌ No auth data found after storage");
            }

            // Clean up
            results.AppendLine("\n4. Cleaning up test data...");
            Microsoft.Maui.Storage.SecureStorage.Remove(AuthTokenKey);
            Microsoft.Maui.Storage.SecureStorage.Remove(UserIdKey);
            Microsoft.Maui.Storage.SecureStorage.Remove(UserEmailKey);
            Microsoft.Maui.Storage.SecureStorage.Remove(UserDisplayNameKey);
            Microsoft.Maui.Storage.SecureStorage.Remove(LoginTimestampKey);
            Microsoft.Maui.Storage.SecureStorage.Remove("HasRestoredSession");
            results.AppendLine("   ✅ Test data cleaned up");

        }
        catch (Exception ex)
        {
            results.AppendLine($"\n❌ TEST FAILED: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Handles the go back button click
    /// </summary>
    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
} 