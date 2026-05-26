# AI Usage Report

## Tools Used
- ChatGPT
- GEMINI


## AI Assisted Tasks
- Project structure cleanup
- Startup null safety improvements
- Restart verification
- Powerup lifecycle validation

## Accepted Suggestions
- Added singleton safety checks
- Added serialized fields
- Added restart reset methods
- Improved coroutine handling

## Modified Suggestions
- Adjusted AI-generated reset logic to fit existing architecture
- Simplified overly complex pooling recommendations

## Rejected Suggestions
- Rejected full gameplay manager rewrite
Reason:
Current architecture already supported assignment requirements.

## Verification Performed
- Tested all powerups individually
- Tested restart during active powerups
- Verified no missing references during startup
- Verified object pooling behavior after restart

## Before / After Notes


## Before
- Scripts were stored in a single folder with limited organization.
- Startup flow could produce null reference issues if references were missing.
- Restart flow did not fully reset runtime gameplay state.
- Runtime validation and defensive checks were minimal.

## After
- Scripts were reorganized into Managers, Gameplay, Spawners, UI, and Data folders.
- Added defensive null validation and startup safety checks.
- Restart flow now properly resets gameplay systems and UI state.
- Powerup activation and expiration behavior were verified and improved.
- Added runtime robustness improvements and safer coroutine handling.