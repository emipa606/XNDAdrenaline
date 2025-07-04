﻿using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace Adrenaline;

public class Hediff_AdrenalineCrash : Hediff_Adrenaline
{
    private float _targetSeverityUnclamped;
    private bool severityFalling;
    private int ticksAtTargetSeverity;

    public int ticksToSeverityGain;

    public AdrenalineCrashProperties Props => def.GetModExtension<HediffDefExtension>().adrenalineCrash;

    private Hediff AdrenalineRushHediff =>
        pawn.health.hediffSet.GetFirstHediffOfDef(ExtraRaceProps.adrenalineRushHediff);

    protected virtual float TargetSeverityUnclamped
    {
        get => Mathf.Max(_targetSeverityUnclamped, 0);
        set => _targetSeverityUnclamped = value;
    }

    protected virtual float SeverityGainFactor =>
        TargetSeverityUnclamped < 1 ? Mathf.Sqrt(TargetSeverityUnclamped) : TargetSeverityUnclamped;

    protected virtual float TargetSeverity => Mathf.Min(TargetSeverityUnclamped, def.maxSeverity);

    public override bool ShouldRemove => base.ShouldRemove && TargetSeverity == 0;

    protected override void UpdateSeverity()
    {
        // Increase the target severity based on the adrenaline rush's severity
        if (AdrenalineRushHediff != null)
        {
            severityFalling = false;
            TargetSeverityUnclamped += AdrenalineRushHediff.Severity *
                                       Props.targetSeverityGainPerAdrenalineRushHediffSeverityPerHour /
                                       GenDate.TicksPerHour *
                                       SeverityUpdateIntervalTicks;
        }

        // If there's a delay in effect, count down the ticks until severity can increase
        if (ticksToSeverityGain > 0)
        {
            ticksToSeverityGain = Mathf.Max(ticksToSeverityGain - SeverityUpdateIntervalTicks, 0);
        }

        // Increase severity if total severity gained is below target severity
        else if (Severity < TargetSeverity)
        {
            Severity += Mathf.Min(TargetSeverity - Severity,
                Props.baseSeverityGainPerDay / GenDate.TicksPerDay * SeverityUpdateIntervalTicks *
                SeverityGainFactor);
        }

        // Otherwise if it's been a certain amount of time since target severity was hit, drop severity
        else
        {
            if (ticksAtTargetSeverity >= (int)(Props.baseTicksAtPeakSeverityBeforeSeverityLoss * Severity) ||
                severityFalling)
            {
                severityFalling = true;
                ticksAtTargetSeverity -= Mathf.Min(ticksAtTargetSeverity, SeverityUpdateIntervalTicks / 2);
                Severity -= Props.baseSeverityLossPerDay / GenDate.TicksPerDay * SeverityUpdateIntervalTicks;
                TargetSeverityUnclamped = Severity;
            }

            else
            {
                ticksAtTargetSeverity += SeverityUpdateIntervalTicks;
            }
        }
    }

    public override string DebugString()
    {
        var debugBuilder = new StringBuilder();
        debugBuilder.AppendLine($"ticksToSeverityGain: {ticksToSeverityGain}".Indented());
        debugBuilder.AppendLine($"unclamped target severity: {TargetSeverityUnclamped}".Indented());
        debugBuilder.AppendLine($"ticks at target severity: {ticksAtTargetSeverity}".Indented());
        debugBuilder.AppendLine($"severityFalling: {severityFalling}".Indented());
        debugBuilder.AppendLine(base.DebugString());
        return debugBuilder.ToString();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref ticksToSeverityGain, "ticksToSeverityGain");
        Scribe_Values.Look(ref _targetSeverityUnclamped, "targetSeverityUnclamped");
        Scribe_Values.Look(ref ticksAtTargetSeverity, "ticksAtTargetSeverity");
        Scribe_Values.Look(ref severityFalling, "severityFalling");

        base.ExposeData();
    }
}