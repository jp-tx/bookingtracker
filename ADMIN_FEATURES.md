# Admin Features

## Overview

BookingTracker now includes admin account management and registration control features.

## Environment Variables

### Docker Compose Configuration

Add these environment variables to your `docker-compose.yml`:

```yaml
environment:
  - ADMIN_EMAIL=admin@bookingtracker.local
  - ADMIN_PASSWORD=ChangeMe123!
  - ALLOW_REGISTRATION=false
```

### Configuration Options

1. **ADMIN_EMAIL** (required)
   - Email address for the admin account
   - Used for login
   - Example: `admin@yourdomain.com`

2. **ADMIN_PASSWORD** (required)
   - Password for the admin account
   - Updated on every application restart
   - Minimum 6 characters
   - Example: `SecurePassword123!`

3. **ALLOW_REGISTRATION** (default: true)
   - Controls whether users can self-register
   - `true`: Registration page is enabled
   - `false`: Registration page shows "disabled" message
   - When disabled, only admins can create accounts

## Admin Account

### Automatic Creation

The admin account is created automatically when the application starts:
- If the account doesn't exist, it's created
- If it exists, the password is updated to match the environment variable
- The account is automatically assigned the "Admin" role

### First Login

1. Navigate to the login page
2. Enter the email from `ADMIN_EMAIL` environment variable
3. Enter the password from `ADMIN_PASSWORD` environment variable
4. You'll be logged in as an administrator

## Admin Features

### User Management Page

After logging in as admin, you'll see an "Admin" link in the navigation bar.

#### Create New Users

1. Click the "Admin" link in the navigation
2. Click "+ Create New User" to expand the form
3. Fill in:
   - **Email**: User's email address (used for login)
   - **Password**: Initial password (minimum 6 characters)
   - **Make this user an administrator**: Check to grant admin privileges
4. Click "Create User"

#### Delete Users

1. Go to the Admin page
2. Find the user in the list
3. Click "Delete" button
4. Confirm the deletion

**Note**: You cannot delete your own account while logged in.

#### View Users

The admin page shows all users with:
- Email address
- Role badges (Admin or User)
- Action buttons

## Registration Control

### Disabling Registration

To prevent public registration:

1. Set `ALLOW_REGISTRATION=false` in docker-compose.yml
2. Restart the application: `docker-compose restart`

When disabled:
- Registration page shows: "Registration is currently disabled"
- Users are directed to contact an administrator
- Only admins can create accounts via the Admin page

### Enabling Registration

To allow public registration:

1. Set `ALLOW_REGISTRATION=true` in docker-compose.yml
2. Restart the application: `docker-compose restart`

## Security Best Practices

1. **Change Default Password**
   - Update `ADMIN_PASSWORD` immediately after first deployment
   - Use a strong password with letters, numbers, and symbols

2. **Keep Password Secure**
   - Store the password securely (password manager)
   - Don't commit passwords to version control
   - Consider using Docker secrets for production

3. **Disable Public Registration**
   - Set `ALLOW_REGISTRATION=false` for production
   - Create accounts manually via Admin page
   - Review and audit user accounts regularly

4. **Regular Updates**
   - Change admin password periodically
   - Update the environment variable and restart the app
   - Password is automatically updated on startup

## Production Deployment

### Example Production Configuration

```yaml
services:
  bookingtracker:
    image: ghcr.io/jp-tx/bookingtracker:latest
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ADMIN_EMAIL=admin@yourdomain.com
      - ADMIN_PASSWORD=${ADMIN_PASSWORD}  # Use Docker secret or .env file
      - ALLOW_REGISTRATION=false
```

### Using Environment File

Create a `.env` file (don't commit this):

```env
ADMIN_PASSWORD=YourSecurePassword123!
```

Reference in docker-compose.yml:

```yaml
services:
  bookingtracker:
    env_file:
      - .env
    environment:
      - ADMIN_EMAIL=admin@yourdomain.com
      - ALLOW_REGISTRATION=false
```

## Troubleshooting

### Can't Login as Admin

1. Check the logs: `docker-compose logs`
2. Verify environment variables are set correctly
3. Restart the container: `docker-compose restart`
4. Try the password from the `ADMIN_PASSWORD` environment variable

### Registration Still Showing When Disabled

1. Check `ALLOW_REGISTRATION` is set to `false` (not `False` or `0`)
2. Restart the container: `docker-compose restart`
3. Clear browser cache and reload

### Admin Link Not Showing

1. Make sure you're logged in as the admin user
2. Check that the account has the Admin role
3. Restart the application to ensure role assignment

### Password Not Updating

- Password is updated on every application startup
- Restart the container after changing the environment variable
- Check logs for any errors during startup
