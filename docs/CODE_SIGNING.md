# Code Signing Notes

Code signing is the strongest practical improvement for SmartScreen reputation and trust when distributing this executable.

## Why it matters

- Unsigned executables are more likely to trigger SmartScreen warnings.
- Signed executables communicate publisher identity.
- Timestamped signatures remain valid after the certificate expires.

## Recommended approach

1. Buy a standard or EV code-signing certificate from a trusted CA.
2. Export the certificate as a `.pfx`.
3. Store the Base64-encoded certificate and password in GitHub Actions secrets.
4. Let the release workflow sign the executable during tagged releases.

## GitHub Actions secrets expected by this repository

- `SIGN_CERT_BASE64`
- `SIGN_CERT_PASSWORD`

If those secrets are not set, the release workflow still builds and publishes the unsigned executable.

## SmartScreen guidance

- EV certificates usually build SmartScreen trust faster.
- Standard certificates still help, but reputation may take longer to build.
- Consistent publisher name, stable releases, and signed artifacts all help over time.
