﻿using SMLHelper.Options;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MoreQuickSlotsBepInEx.Config.CustomKeyboardShortcut;

namespace MoreQuickSlotsBepInEx.Config
{
    internal class SMLConfig : ModOptions
    {
        public SMLConfig() : base("More Quick Slots (BepInEx)")
        {
            OnChanged += Options_Changed;
        }

        public override void BuildModOptions()
        {
            MoreQuickSlotsBepInEx.logger.LogInfo("BUILDING MOD OPTIONS");
            AddItem(ModToggleOption.Factory("DAATQS", "Disable Auto-Bind", BepInExConfig.DAATQS.Value));
            AddItem(ModSliderOption.Factory("Extra Slots", "Extra Slots", 0, 15, BepInExConfig.ExtraSlots.Value, 4, "{0}", 1));

            for (int i = 0; i < BepInExConfig.MAX_EXTRA_SLOTS; i++)
            {
                AddItem(ModChoiceOption.Factory("ExtraHotkey" + i.ToString().PadLeft(2, '0'), "Quickslot " + (i + 6) + " Hotkey", (AllowedKeys)BepInExConfig.SlotHotkeys[i].MainKey));
                AddItem(CustomKeyBoardModifiersOption.Factory($"ExtraHotkey {i.ToString().PadLeft(2, '0')}MOD", $"Quickslot {(i + 6)} Modifiers", BepInExConfig.SlotHotkeys[i]));
            }
            AddItem(ModButtonOption.Factory("Button_1", "Factory Button", (ButtonClickedEventArgs e) => MoreQuickSlotsBepInEx.logger.LogInfo("Factory Button Clicked")));
            AddItem(ModColorOption.Factory("Color_1", "Test Color", Color.white));

            AddItem(ModSliderOption.Factory("ID1", "Default of value", 0, 10, 5));
            AddItem(ModSliderOption.Factory("ID2", "Default of 3", 0, 10, 5, 3, "{0:F0}", 2));
        }

        private void Options_Changed(object sender, EventArgs e)
        {
            switch (e)
            {
                case ToggleChangedEventArgs args:
                    switch (args.Id)
                    {
                        case "DAATQS":
                            BepInExConfig.DAATQS.Value = args.Value;
                            break;
                    }
                    break;
                case SliderChangedEventArgs args:
                    switch (args.Id)
                    {
                        case "Extra Slots":
                            BepInExConfig.ExtraSlots.Value = (int)args.Value;
                            break;
                    }
                    break;
                case ChoiceChangedEventArgs args:
                    if (args.Id.StartsWith("ExtraHotkey"))
                    {
                        int hotkeyNumber = int.Parse(args.Id.Substring(args.Id.Length - 2));
                        BepInExConfig.SlotHotkeys[hotkeyNumber]._MainKey.Value = (AllowedKeys)Enum.Parse(typeof(AllowedKeys), args.Value.Value);
                    }
                    break;
                case ColorChangedEventArgs args:
                    MoreQuickSlotsBepInEx.logger.LogInfo($"Changed color to {args.Value}");
                    break;
            }
        }
    }

    internal class CustomKeyBoardModifiersEventArgs : ConfigOptionEventArgs<string>
    {
        internal CustomKeyBoardModifiersEventArgs(string id, string values) : base(id, values) { }
    }

    internal class CustomKeyBoardModifiersOption : ModOption<string>
    {
        private readonly CustomKeyboardShortcut _BackingField;

        public override void AddToPanel(uGUI_TabbedControlsPanel panel, int tabIndex)
        {
            Canvas canvas = new GameObject("Canvas", typeof(RectTransform)).AddComponent<Canvas>();

            GameObject gameObject = panel.AddItem(tabIndex, panel.toggleOptionPrefab, Label);
            Toggle toggle = gameObject.EnsureComponent<Toggle>();
            toggle.isOn = true;
            TextMeshProUGUI text = gameObject.EnsureComponent<TextMeshProUGUI>();
            text.text = "Test";

            //return componentInChildren1;
            //var modifiers = _BackingField.GetModifiers; // Fetch to consolidate string value with modifiers value
            //foreach (AllowedModifiers modifier in Enum.GetValues(typeof(AllowedModifiers)))
            //{
            //    UnityEngine.UI.Toggle toggle = panel.AddToggleOption(tabIndex, modifier.ToString(), modifiers[modifier],
            //        new UnityAction<bool>((bool value) =>
            //        {
            //            _BackingField.SetModifier(modifier, value);
            //        }));
            //}
            panel.AddItem(tabIndex, canvas.gameObject, Label);
        }

        private CustomKeyBoardModifiersOption(string id, string label, CustomKeyboardShortcut shortcut) : base(id, label, shortcut.ModifiersString)
        {
            _BackingField = shortcut;
        }

        public static CustomKeyBoardModifiersOption Factory(string id, string label, CustomKeyboardShortcut shortcut)
        {
            return new CustomKeyBoardModifiersOption(id, label, shortcut);
        }

        //private class CustomKeyBoardModifiersOptionAdjust : ModOptionAdjust
        //{
        //    private const float spacing = 20f;

        //    public IEnumerator Start()
        //    {
        //        SetCaptionGameObject("Toggle/Caption");
        //        yield return null;

        //        Transform check = gameObject.transform.Find("Toggle/Background");

        //        if (CaptionWidth + spacing > check.localPosition.x)
        //        {
        //            check.localPosition = SetVec2x(check.localPosition, CaptionWidth + spacing);
        //        }

        //        Destroy(this);
        //    }
        //}
        public override Type AdjusterComponent => null;

    }
}
