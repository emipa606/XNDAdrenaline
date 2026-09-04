# GitHub Copilot Instructions for [XND] Adrenaline! (Continued)

## Mod Overview and Purpose

The [XND] Adrenaline! (Continued) mod enhances the combat experience in RimWorld by introducing an adrenaline system for biological pawns. The mod adds realism and excitement by making pawns experience adrenaline surges during fight-or-flight situations. This system ensures that all biological entities, from colonists to wildlife, can be influenced by adrenaline, heightening the immersive war-time experience in the game.

## Key Features and Systems

- **Adrenaline System:** Pawns gain boosts in consciousness, movement, and melee damage during adrenaline surges, while experiencing reduced pain.
- **Adrenaline Crashes:** Prolonged adrenaline surges can lead to crashes, causing brief periods of lethargy in pawns.
- **Personality Traits:** Pawns can be adrenaline junkies who thrive intensely, or cool-headed individuals who remain unaffected by high-pressure scenarios.
- **Adrenaline Syringes:** Downed pawns with such syringes may self-inject to recover momentarily, adding strategic depth to combat. Raiders can utilize syringes too.
- **Mod Configuration:** Users can customize their experience by toggling features such as natural adrenaline gains, animal-specific adrenaline, crashes, and raider behaviors.
- **Synthesis and Compatibility:** Adrenaline can be crafted at drug labs, and mods like Smart Medicine and Combat Extended enhance the experience further.

## Coding Patterns and Conventions

- **Naming Conventions:** Use PascalCase for types and methods (`CompAdrenalineTracker`, `AdrenalineProduced`), and camelCase for local variables and parameters.
- **Modular Structure:** Keep separate classes and definitions for components, properties, patches, and XML definitions.
- **Consistent Indentation:** Ensure consistent indentation across all code files for readability.
- **Commenting:** Code should be sufficiently commented to describe method functionalities and purpose.

## XML Integration

XML files structure the configuration of various game elements;

- Define **HediffDefs** for pawn state changes in `Hediffs_Global_Misc.xml`.
- Use **StatDefs** to adjust combat statistics in `Stats_Pawns_Combat.xml`.
- Create or modify **ThingDefs** for items like adrenaline syringes in `Adrenaline.xml`.
- Establish **ThoughtDefs** for mood effects tied to traits in `Thoughts_Situational.xml`.
- Set up **TraitDefs** that handle pawn characteristics in `Traits_Singular.xml`.

## Harmony Patching

- The mod uses the Harmony Patch Library for runtime modifications without altering the core game code.
- Ensure all patches respect the Harmony standards for prefix, postfix, and transpiler patches.
- Review patches in `CompAdrenalineTracker.cs` to see how existing game methods are extended seamlessly.

## Suggestions for Copilot

- **Intelligent Suggestions:** Suggest context-specific method and class usages, drawing from existing patterns in `CompAdrenalineTracker.cs` and `Adrenaline.cs`.
- **Auto-generate XML:** Recommend baseline XML structure for new Defs when creating features like additional traits or item modifications.
- **Error Handlers:** Propose best practices for exception handling in C# files that integrate with the Harmony library.
- **Enhance Mod Compatibility:** Suggest code structures that integrate smoothly with other popular mods.
- **Optimize Settings Interface:** Provide suggestions to simplify settings modifications in `DoSettingsWindowContents`.

These instructions for GitHub Copilot aim to optimize the process of contributing to or customizing [XND] Adrenaline! (Continued), aligning with the existing structures and enhancing user experience effectively.

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

