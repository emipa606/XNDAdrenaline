# .github/copilot-instructions.md

## [XND] Adrenaline! (Continued) Mod Overview

**Purpose of the Mod:**
The [XND] Adrenaline! mod aims to enhance combat realism and immersion in RimWorld by simulating adrenaline production in biological pawns during fight-or-flight scenarios. Adrenaline affects pawns universally, from colonists to wildlife, and creates a more dynamic experience in combat situations.

## Key Features and Systems

1. **Adrenaline Effects:**
   - Boosts consciousness, movement, and melee damage.
   - Reduces felt pain during the rush.
   - Extended rushes lead to adrenaline crashes, causing lethargy.

2. **Adrenaline Traits:**
   - Adrenaline junkies: Pawns who get mood boosts from combat.
   - Cool-headed: Pawns unaffected by adrenaline surges.

3. **Adrenaline Syringes:**
   - Usable by downed pawns to re-enter combat.
   - Synthesizable post penoxycyline research.

4. **Customization & Compatibility:**
   - Toggle features in Mod Settings.
   - Compatible with mods like Smart Medicine and Combat Extended.
   - Works with alien races and modded animals.
   - Check the GitHub wiki for modding adrenaline characteristics.

## Coding Patterns and Conventions

- Classes are organized with clear responsibilities such as `CompAdrenalineTracker`, `CompProperties_AdrenalineTracker`, and `AdrenalineRushProperties`.
- Use of public and static classes for generalized utility and modularity (`AdrenalineUtility`, `HarmonyPatches`).
- Clear naming conventions prefixed with `A_` for definitions and constants (e.g., `A_ConceptDefOf`, `A_HediffDefOf`).

## XML Integration

- XML is used for defining traits, hediffs, stats, and other game entities.
- Modders can extend or define custom behavior by editing XML configurations, especially for unique race or animal behaviors.
- `DefModExtension` classes like `ExtendedRaceProperties` are used for supporting additional XML properties.

## Harmony Patching

- Harmony is extensively used for runtime code alterations.
- Patches are organized into clear extension methods with descriptive filenames and method names (`PawnUtility_IsFighting`, `Thing_SpawnSetup`).
- It ensures compatibility across updates by modifying game code non-invasively and safely.

## Suggestions for Copilot

1. **Code Generation:**
   - Generate new components or extensions by following existing naming and structural conventions.
   - Recommend code snippets for common patterns observed in mod classes.

2. **XML Definition:**
   - Assist in generating XML files based on existing def structures, ensuring proper schema usage.

3. **Harmony Patching:**
   - Suggest starter templates for new Harmony patches, following the pattern of existing hooks.
   - Offer patterns for common tasks like modifying pawn behaviors or adding new game interactions.

4. **Debugging and Error Handling:**
   - Provide tips on logging and debugging within C# for RimWorld context.
   - Recommend best practices for handling errors when modifying or removing components.

By keeping these instructions in mind, developers can leverage GitHub Copilot to enhance and maintain the [XND] Adrenaline! mod effectively, ensuring both quality and consistency in their contributions.
