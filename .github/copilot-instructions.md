# GitHub Copilot Instructions for [XND] Adrenaline! (Continued)

## Mod Overview and Purpose

**[XND] Adrenaline! (Continued)** is a RimWorld mod designed to enhance the combat experience by introducing adrenaline rush mechanics for biological pawns. The mod enriches immersion by simulating how adrenaline affects consciousness, movement, melee damage, and pain tolerance. After the adrenaline rush, pawns may experience crashes, simulating fatigue. This mod not only intensifies the combat situations but also allows for unique behavioral traits like "adrenaline junkies" and "cool-headed" individuals, adding layers to pawn interaction and strategy.

## Key Features and Systems

- **Adrenaline Rush Mechanic**: Biological pawns receive boosts in consciousness, movement, and melee damage when engaged in intense situations. This feature simulates the fight-or-flight response.
- **Adrenaline Crash**: Following a period of enhanced performance, pawns experience a duration of lethargy simulating fatigue post-rush.
- **Behavioral Traits**: Some pawns enjoy the thrill (adrenaline junkies), while others remain unaffected (cool-headed).
- **Adrenaline Syringe**: Items that can revive downed pawns, providing an additional strategy layer in battles.
- **Mod Settings Customization**: Allows players to disable specific mod components like natural gains, crashes, or raider capabilities.
- **Compatibility**: Works seamlessly with alien races and modded animals. Suggested for use with "Smart Medicine" and "Combat Extended" mods.

## Coding Patterns and Conventions

### C# Coding Patterns

- **Modular Codebase**: Separation of functionality into different classes, e.g., `AdrenalineUtility`, `CompAdrenalineTracker`.
- **Adoption of Harmony Library**: Utilized for patching methods without altering original game code.
- **Naming Conventions**: Descriptive and consistent names for classes, methods (e.g., `CanGetAdrenaline`, `AdrenalineProduced`).
- **Encapsulation**: Proper use of private, public, and protected access modifiers to control data access.

### XML Integration

- **Defs Usage**: `HediffDef`, `StatDef`, `ThingDef`, `ThoughtDef`, and `TraitDef` integrated in various XML files for defining new data elements.
- **Structured XML Files**: Organized as per functionalities (e.g., `Hediffs_Global_Misc.xml`, `Stats_Pawns_Combat.xml`) to ensure maintainability and clarity.

## Harmony Patching

- Making use of the **Harmony Patch Library** for non-intrusive patches.
- Ensure that any patches are well-documented within the source code to maintain clarity.
- Typical usage involves prefix, postfix, or transpiler methods depending on the needed instruction modifications.

## Suggestions for Copilot

- **Autocompletion**: Enhance efficiency by suggesting code structures based on frequent patterns in similar mods, like method signatures in `AdrenalineUtility`.
- **XML Schema**: Offer suggestions for XML schema definitions based on existing RimWorld mods.
- **Harmony Patch Patterns**: Propose common patching practices, ensuring compatibility across different game updates.
- **Code Comments**: Auto-generate code comments based on function documentation to ensure code clarity.
- **Refactoring Suggestions**: Propose potential code refactors to improve performance or readability.
- **Testing Suggestion**: Recommend automated test practices for both XML and C# components to ensure stability when integrated into gameplay.

By following these practices and utilizing Copilot's capabilities, the development and maintenance of the [XND] Adrenaline! (Continued) mod can be more efficient, allowing a streamlined modding experience that positively impacts gameplay.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

