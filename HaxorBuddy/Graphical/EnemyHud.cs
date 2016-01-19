using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace HaxorBuddy.Graphical
{
    class EnemyHud : Mode
    {
        private int SpaceBetweenChamps = 31;
        private int XOffset = 20;
        private int SpaceFromTop = Drawing.Height - 160;
        private int SpaceFromChampName = 60;
        private int SpaceBetweenCD = 19;

        private readonly SpellSlot[] SpellsSlots =
            { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R};
        private readonly SpellSlot[] SummsSlots =
            { SpellSlot.Summoner1, SpellSlot.Summoner2 };

        private Menu ehMenu;

        private readonly string[] summNames = new string[] {
            "summonerbarrier", "summonerboost", "summonerdot", "summonerexhaust",
            "summonerflash", "summonerhaste", "summonerheal", "summonersmite",
            "summonerteleport" };

        private bool Loaded = false;
        private TextureLoader Loader = new TextureLoader();
        private Dictionary<string, Sprite> SummonerSpellsIcons = new Dictionary<string, Sprite>();
        private Sprite EnemyHUDBackground;

        public override void CreateMenu()
        {
            /*
            ehMenu = HaxorMenu.haxorMenu.AddSubMenu("Enemy HUD");

            ehMenu.AddGroupLabel("UI Design");

            ehMenu.Add("deBC", new Slider("Pixels between champ names (default: 25)", SpaceBetweenChamps))
                .OnValueChange += (sender, args) => { SpaceBetweenChamps = args.NewValue; };

            ehMenu.Add("deXOff", new Slider("Pixels from the left of the screen (default: 20)", XOffset))
                .OnValueChange += (sender, args) => { XOffset = args.NewValue; };

            ehMenu.Add("deSFT", new Slider("Pixels from the top of the screen (default: 50)", SpaceFromTop))
                .OnValueChange += (sender, args) => { SpaceFromTop = args.NewValue; };

            ehMenu.Add("deSFCN", new Slider("[Cooldowns] Pixels from the champion name (default: 60)", SpaceFromChampName))
                .OnValueChange += (sender, args) => { SpaceFromChampName = args.NewValue; };

            ehMenu.Add("deBCD", new Slider("[Cooldowns] Pixels between cooldown spells (default: 20)", SpaceBetweenCD))
                .OnValueChange += (sender, args) => { SpaceBetweenCD = args.NewValue; };
            */
        }

        public override bool DefaultEnabled()
        {
            return false;
        }

        public override string GetID()
        {
            return "Enemy HUD";
        }

        private Text champName, cdText, hpText, summText;

        public override bool Init()
        {
            if (!Loaded)
            {
                var res = new ResourceManager(typeof(Resources));

                foreach (var item in summNames)
                {
                    Loader.Load(item, (Bitmap)res.GetObject(item));
                    SummonerSpellsIcons.Add(item, new Sprite(() => Loader[item]));
                }

                Loader.Load("enemyhudbg", res.GetObject("enemyhud") as Bitmap);
                EnemyHUDBackground = new Sprite(() => Loader["enemyhudbg"]);

                Loaded = true;
            }
            
            champName = new Text(string.Empty, new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold));
            cdText = new Text(string.Empty, new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular));
            hpText = new Text(string.Empty, new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold));
            summText = new Text(string.Empty, new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular));

            Drawing.OnEndScene += Drawing_OnEndScene;
            
            return true;
        }

        private void Drawing_OnEndScene(EventArgs args)
        {
            int index = 0;
            int sindex = 0;

            EnemyHUDBackground.Draw(new Vector2(30, SpaceFromTop - 1));

            foreach (var champ in EntityManager.Heroes.Enemies)
            {
                int yOffset = index * SpaceBetweenChamps + SpaceFromTop + 2;
                Vector2 infoPos = new Vector2(31, yOffset);
                
                //Champ name

                champName.Draw(champ.ChampionName, Color.Goldenrod, infoPos);

                
                //Cooldowns
                //Champion spells
                foreach (var slot in SpellsSlots)
                {
                    Color color;
                    cdText.Draw(GetSpellString(champ, slot, out color),
                        ToSharpDXColor(color), (int)infoPos.X + SpaceFromChampName +
                        sindex * SpaceBetweenCD, (int)infoPos.Y);
                    sindex++;
                }
                sindex = 0;

                //Health
                hpText.Draw(((int)champ.HealthPercent).ToString() + "% HP", SharpDX.Color.Green,
                    (int)infoPos.X + 3, (int)infoPos.Y + 10);

                //Summoner spells
                foreach (var slot in SummsSlots)
                {
                    var summPos = new Vector2(sindex * 30 + 170, yOffset);
                    var spell = champ.Spellbook.GetSpell(slot);
                    var cooldown = spell.CooldownExpires - Game.Time;
                    if (cooldown < 0) cooldown = 0;

                    SummonerSpellsIcons[spell.Name].Draw(summPos);

                    summText.Draw(Math.Ceiling(cooldown).ToString(),
                        Color.LawnGreen, (int)summPos.X, (int)summPos.Y + 17);

                    sindex++;
                }
                sindex = 0;

                
                index++;
            }
        }

        public override void Stop()
        {
            Drawing.OnEndScene -= Drawing_OnEndScene;
        }

        private string GetSpellString(AIHeroClient hero, SpellSlot slot, out Color txtcolor)
        {
            var spell = hero.Spellbook.GetSpell(slot);
            var color = Color.Green;
            var cooldown = spell.CooldownExpires - Game.Time;

            var str = slot.ToString();

            if (!spell.IsLearned)
            {
                str = "X";
                color = Color.Red;
            }
            else if (hero.Mana < spell.SData.Mana)
            {
                str = "M";
                color = Color.LightBlue;
            }
            else if (cooldown > 0)
            {
                str = ((int)Math.Ceiling(cooldown)).ToString();
                color = Color.Orange;
            }

            txtcolor = color;
            return str;
        }

        private SharpDX.Color ToSharpDXColor(Color color)
        {
            return new SharpDX.Color(color.R, color.G, color.B, color.A);
        }
    }
}
