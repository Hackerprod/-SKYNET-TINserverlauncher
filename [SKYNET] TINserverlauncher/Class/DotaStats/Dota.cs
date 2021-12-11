using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TINserverlauncher
{
    public class Dota
	{
        public string HeroName { get; set; } //heroes2.json
        public String FindHero(string heroToFind)
        {
            //find hero object 1 (this one has most of the stats)
            switch (heroToFind)
            {
                case "npc_dota_hero_antimage":
                    HeroName = "Anti-Mage"; break;
                case "npc_dota_hero_axe":
                    HeroName = "Axe"; break;
                case "npc_dota_hero_bane":
                    HeroName = "Bane"; break;
                case "npc_dota_hero_bloodseeker":
                    HeroName = "Bloodseeker"; break;
                case "npc_dota_hero_crystal_maiden":
                    HeroName = "Crystal Maiden"; break;
                case "npc_dota_hero_drow_ranger":
                    HeroName = "Drow Ranger"; break;
                case "npc_dota_hero_earthshaker":
                    HeroName = "Earthshaker"; break;
                case "npc_dota_hero_juggernaut":
                    HeroName = "Juggernaut"; break;
                case "npc_dota_hero_mirana":
                    HeroName = "Mirana"; break;
                case "npc_dota_hero_nevermore":
                    HeroName = "Shadow Fiend"; break;
                case "npc_dota_hero_morphling":
                    HeroName = "Morphling"; break;
                case "npc_dota_hero_phantom_lancer":
                    HeroName = "Phantom Lancer"; break;
                case "npc_dota_hero_puck":
                    HeroName = "Puck"; break;
                case "npc_dota_hero_pudge":
                    HeroName = "Pudge"; break;
                case "npc_dota_hero_razor":
                    HeroName = "Razor"; break;
                case "npc_dota_hero_sand_king":
                    HeroName = "Sand King"; break;
                case "npc_dota_hero_storm_spirit":
                    HeroName = "Storm Spirit"; break;
                case "npc_dota_hero_sven":
                    HeroName = "Sven"; break;
                case "npc_dota_hero_tiny":
                    HeroName = "Tiny"; break;
                case "npc_dota_hero_vengefulspirit":
                    HeroName = "Vengeful Spirit"; break;
                case "npc_dota_hero_windrunner":
                    HeroName = "Windranger"; break;
                case "npc_dota_hero_zuus":
                    HeroName = "Zeus"; break;
                case "npc_dota_hero_kunkka":
                    HeroName = "Kunkka"; break;
                case "npc_dota_hero_lina":
                    HeroName = "Lina"; break;
                case "npc_dota_hero_lich":
                    HeroName = "Lich"; break;
                case "npc_dota_hero_lion":
                    HeroName = "Lion"; break;
                case "npc_dota_hero_shadow_shaman":
                    HeroName = "Shadow Shaman"; break;
                case "npc_dota_hero_slardar":
                    HeroName = "Slardar"; break;
                case "npc_dota_hero_tidehunter":
                    HeroName = "Tidehunter"; break;
                case "npc_dota_hero_witch_doctor":
                    HeroName = "Witch Doctor"; break;
                case "npc_dota_hero_riki":
                    HeroName = "Riki"; break;
                case "npc_dota_hero_enigma":
                    HeroName = "Enigma"; break;
                case "npc_dota_hero_tinker":
                    HeroName = "Tinker"; break;
                case "npc_dota_hero_sniper":
                    HeroName = "Sniper"; break;
                case "npc_dota_hero_necrolyte":
                    HeroName = "Necrophos"; break;
                case "npc_dota_hero_warlock":
                    HeroName = "Warlock"; break;
                case "npc_dota_hero_beastmaster":
                    HeroName = "Beastmaster"; break;
                case "npc_dota_hero_queenofpain":
                    HeroName = "Queen of Pain"; break;
                case "npc_dota_hero_venomancer":
                    HeroName = "Venomancer"; break;
                case "npc_dota_hero_faceless_void":
                    HeroName = "Faceless Void"; break;
                case "npc_dota_hero_skeleton_king":
                    HeroName = "Wraith King"; break;
                case "npc_dota_hero_death_prophet":
                    HeroName = "Death Prophet"; break;
                case "npc_dota_hero_phantom_assassin":
                    HeroName = "Phantom Assassin"; break;
                case "npc_dota_hero_pugna":
                    HeroName = "Pugna"; break;
                case "npc_dota_hero_templar_assassin":
                    HeroName = "Templar Assassin"; break;
                case "npc_dota_hero_viper":
                    HeroName = "Viper"; break;
                case "npc_dota_hero_luna":
                    HeroName = "Luna"; break;
                case "npc_dota_hero_dragon_knight":
                    HeroName = "Dragon Knight"; break;
                case "npc_dota_hero_dazzle":
                    HeroName = "Dazzle"; break;
                case "npc_dota_hero_rattletrap":
                    HeroName = "Clockwerk"; break;
                case "npc_dota_hero_leshrac":
                    HeroName = "Leshrac"; break;
                case "npc_dota_hero_furion":
                    HeroName = "Nature's Prophet"; break;
                case "npc_dota_hero_life_stealer":
                    HeroName = "Lifestealer"; break;
                case "npc_dota_hero_dark_seer":
                    HeroName = "Dark Seer"; break;
                case "npc_dota_hero_clinkz":
                    HeroName = "Clinkz"; break;
                case "npc_dota_hero_omniknight":
                    HeroName = "Omniknight"; break;
                case "npc_dota_hero_enchantress":
                    HeroName = "Enchantress"; break;
                case "npc_dota_hero_huskar":
                    HeroName = "Huskar"; break;
                case "npc_dota_hero_night_stalker":
                    HeroName = "Night Stalker"; break;
                case "npc_dota_hero_broodmother":
                    HeroName = "Broodmother"; break;
                case "npc_dota_hero_bounty_hunter":
                    HeroName = "Bounty Hunter"; break;
                case "npc_dota_hero_weaver":
                    HeroName = "Weaver"; break;
                case "npc_dota_hero_jakiro":
                    HeroName = "Jakiro"; break;
                case "npc_dota_hero_batrider":
                    HeroName = "Batrider"; break;
                case "npc_dota_hero_chen":
                    HeroName = "Chen"; break;
                case "npc_dota_hero_spectre":
                    HeroName = "Spectre"; break;
                case "npc_dota_hero_doom_bringer":
                    HeroName = "Doom"; break;
                case "npc_dota_hero_ancient_apparition":
                    HeroName = "Ancient Apparition"; break;
                case "npc_dota_hero_ursa":
                    HeroName = "Ursa"; break;
                case "npc_dota_hero_spirit_breaker":
                    HeroName = "Spirit Breaker"; break;
                case "npc_dota_hero_gyrocopter":
                    HeroName = "Gyrocopter"; break;
                case "npc_dota_hero_alchemist":
                    HeroName = "Alchemist"; break;
                case "npc_dota_hero_invoker":
                    HeroName = "Invoker"; break;
                case "npc_dota_hero_silencer":
                    HeroName = "Silencer"; break;
                case "npc_dota_hero_obsidian_destroyer":
                    HeroName = "Outworld Devourer"; break;
                case "npc_dota_hero_lycan":
                    HeroName = "Lycan"; break;
                case "npc_dota_hero_brewmaster":
                    HeroName = "Brewmaster"; break;
                case "npc_dota_hero_shadow_demon":
                    HeroName = "Shadow Demon"; break;
                case "npc_dota_hero_lone_druid":
                    HeroName = "Lone Druid"; break;
                case "npc_dota_hero_chaos_knight":
                    HeroName = "Chaos Knight"; break;
                case "npc_dota_hero_meepo":
                    HeroName = "Meepo"; break;
                case "npc_dota_hero_treant":
                    HeroName = "Treant Protector"; break;
                case "npc_dota_hero_ogre_magi":
                    HeroName = "Ogre Magi"; break;
                case "npc_dota_hero_undying":
                    HeroName = "Undying"; break;
                case "npc_dota_hero_rubick":
                    HeroName = "Rubick"; break;
                case "npc_dota_hero_disruptor":
                    HeroName = "Disruptor"; break;
                case "npc_dota_hero_nyx_assassin":
                    HeroName = "Nyx Assassin"; break;
                case "npc_dota_hero_naga_siren":
                    HeroName = "Naga Siren"; break;
                case "npc_dota_hero_keeper_of_the_light":
                    HeroName = "Keeper of the Light"; break;
                case "npc_dota_hero_wisp":
                    HeroName = "Io"; break;
                case "npc_dota_hero_visage":
                    HeroName = "Visage"; break;
                case "npc_dota_hero_slark":
                    HeroName = "Slark"; break;
                case "npc_dota_hero_medusa":
                    HeroName = "Medusa"; break;
                case "npc_dota_hero_troll_warlord":
                    HeroName = "Troll Warlord"; break;
                case "npc_dota_hero_centaur":
                    HeroName = "Centaur Warrunner"; break;
                case "npc_dota_hero_magnataur":
                    HeroName = "Magnus"; break;
                case "npc_dota_hero_shredder":
                    HeroName = "Timbersaw"; break;
                case "npc_dota_hero_bristleback":
                    HeroName = "Bristleback"; break;
                case "npc_dota_hero_tusk":
                    HeroName = "Tusk"; break;
                case "npc_dota_hero_skywrath_mage":
                    HeroName = "Skywrath Mage"; break;
                case "npc_dota_hero_abaddon":
                    HeroName = "Abaddon"; break;
                case "npc_dota_hero_elder_titan":
                    HeroName = "Elder Titan"; break;
                case "npc_dota_hero_legion_commander":
                    HeroName = "Legion Commander"; break;
                case "npc_dota_hero_ember_spirit":
                    HeroName = "Ember Spirit"; break;
                case "npc_dota_hero_earth_spirit":
                    HeroName = "Earth Spirit"; break;
                case "npc_dota_hero_terrorblade":
                    HeroName = "Terrorblade"; break;
                case "npc_dota_hero_phoenix": 
                    HeroName = "Phoenix"; break;
                case "npc_dota_hero_oracle":
                    HeroName = "Oracle"; break;
                case "npc_dota_hero_techies":
                    HeroName = "Techies"; break;
                case "npc_dota_hero_winter_wyvern":
                    HeroName = "Winter Wyvern"; break;
                case "npc_dota_hero_arc_warden":
                    HeroName = "Arc Warden"; break;
                case "npc_dota_hero_abyssal_underlord":
                    HeroName = "Underlord"; break;
                case "npc_dota_hero_monkey_king":
                    HeroName = "Monkey King"; break;
                case "npc_dota_hero_pangolier":
                    HeroName = "Pangolier"; break;
                case "npc_dota_hero_dark_willow":
                    HeroName = "Dark Willow"; break;

            };
            return HeroName;
            
        }


    }

}
