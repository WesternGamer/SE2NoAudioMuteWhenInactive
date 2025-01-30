using HarmonyLib;
using Keen.VRage.Audio.EngineComponents;
using System.Reflection.Emit;
using System.Reflection;
using Keen.VRage.Library.Extensions;

namespace SE2NoAudioMuteWhenInactive.Patches
{
    [HarmonyPatch(typeof(AudioEngineComponent), "AudioManagerTick")]
    internal class AudioEngineComponent_AudioManagerTick_Patch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var il = instructions.ToList();

            int foundIndex = il.FindIndex(i => i.opcode == OpCodes.Callvirt && (i.operand as MethodInfo) == AccessTools.Method("Keen.VRage.Audio.AudioManager:Update"));

            object audManagerInstanceGetterOperand = il[foundIndex - 1].operand;

            List<CodeInstruction> newIl =
                [
                new CodeInstruction(OpCodes.Call, audManagerInstanceGetterOperand),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method("Keen.VRage.Audio.AudioManager:Update")),
                new CodeInstruction(OpCodes.Ret)
                ];

            return newIl;
        }
    }
}
