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

### Before
- Restart did not clear all gameplay states
- Startup could produce null reference errors

### After
- Restart fully resets runtime systems
- Added defensive checks and validation during startup
- Improved powerup lifecycle reliability