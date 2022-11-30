# SoldiersPiratesAssassinsMercs

**Depends On MissionControl and IRBTModUtils!** 

This mod breathes a little life into the mercenaries of the Inner Sphere, allowing mercenary factions to jump in and replace the opfor at contract start. Depending on the configuration, the hostile mercs may replace the opfor entirely, or they may only replace AdditionalLances added by MissionControl. The lance difficulty *should* be approximately the same, as the mod works by simply changing the faction for the replaced units and re-selecting appropriate lances and units. In addition, merc dialogue will be generated at contract start, and the capability to bribe/pay off the hostile mercs has also been implemented. Settings for this mod reside in two places; basic mod settings are in the mod.json, while Dialogue.json contains settings and configuration for dialogue (more on that later).

mod.json settings:
```
"Settings": {
		"enableDebug": true,
		"enableTrace": true,
		"OpforReplacementConfig": {
			"BaseReplaceChance": 0.0,
			"FactionsReplaceOverrides": {
				"Locals": 0.0
			},
			"BlacklistContractTypes": [],
			"BlacklistContractIDs": []
		},
		"MercLanceAdditionConfig": {
			"BaseReplaceChance": 1.0,
			"FactionsReplaceOverrides": {
				"Locals": 0.0
			},
			"BlacklistContractTypes": [],
			"BlacklistContractIDs": [],
			"MercFactionReputationFactor": 0.5
		},
		"MercFactionConfigs": {
			"KellHounds": {
				"MercFactionName": "KellHounds",
				"AppearanceWeight": 1,
				"EmployerBlacklist": [
					"Liao"
				],
				"UnitRating": 1,
				"PersonalityAttributes": [
					"PROFESSIONAL"
				]
			}
		},
		"FallbackUnitFactionTag": "mercenaries",
		"BribeCostBaselineMulti": 0.01
	},
```

`enableDebug` and `enableTrace` - bools, enable debug and trace logging, respectively. recommend leaving debug log disabled and trace enabled.

`OpforReplacementConfig` and `MercLanceAdditionConfig` - configs for whole-opfor and MC AdditionalLances faction replacement, respectively.
	-	`BaseReplaceChance` - baseline chance for any opfor to be replaced by a valid merc faction. uses 0 - 1 range; 0 is never, 1 is guaranteed.
	-	`FactionsReplaceOverrides` - list of faction-specific overrides for the replacement chance. i.e, override Locals and Wobbies to hire mercs more often and override clans to never use them. uses 0 - 1 range; 0 is never, 1 is guaranteed.
	-	`BlacklistContractTypes` and `BlacklistContractIDs` - lists of contract types and specific contract IDs which will never replace opfor with mercs.
	-	`MercFactionReputationFactor` (in MercLanceAdditionConfig only) - custom reputation loss multiplier for hostile merc faction when they only replaced MC AdditionalLances (rep loss for original target faction is always maintained, and when whole opfor is
 replaced by mercs, both merc faction and contract target faction lose rep as normal). Mercenary faction reputation isn't hooked to anything besides regular rep system, but its there if you need it.
 
 `MercFactionConfigs` - Dictionary of configs for Merc Faction behavior. Merc factions *must* be real, valid factions. They have to have a FactionDef, and be defined in Faction.json. In particular, Faction.json must have them with IsRealFaction and IsMercenary set to true. They do *not* have to be set to gain reputation, nor do they need to be IsCareerStartingDisplayFaction and display in the Cpt Quarters reputation screen.
 
 The KEYS for this dictionary are the faction Names for the configured merc factions, i.e KellHounds (same field as Liao, Davion, Locals, etc). If a merc faction exists but is missing from this dictionary, it will not be used by SPAM!
	-	`MercFactionName` - string, should be same as the KEY; the faction Name.
	-	`AppearanceWeight` - integer weight for this merc faction to be used for replacement (relative to other configured merc factions)
	-	`EmployerBlacklist` - list, strings; "employer" blacklist for merc faction. I.e KellHounds will never replace (be hired) by Liao.
	-	`UnitRating` - integer MRB unit rating for the merc faction. Calculates based on a 12 point system, as MRB ratings are given as an A+ through D- scale. Merc Units that are Not Rated get a 0, D- gets one, A+ gets 12.
	-	`PersonalityAttributes` - list of PersonalityAttributes that will determine the "pool" of randomly generated dialogue pulled from the Dialogue.json

`FallbackUnitFactionTag` - fallback unit tag to use if the unit replacer cannot find a unit that matches the lance/unit selector tag for the chosen merc faction. Best to use your most "generic" mercenary faction tag, but de-capitalized. Really just to prevent fallback cicadas while hopefully still being merc-ey.
`BribeCostBaselineMulti` - baseline multiplier for calculation of bribes; multiplied by the total cost (battlevalue) of the mercs you're trying to bribe.


Dialogue.json consists of a Dictionary, where the KEYS are PersonalityAttributes you've decided to use for your merc factions. They can be literally whatever you want; there just needs to be a match between the PersonalityAttributes in mod.json and the KEYS in Dialogue.json. They also don't have to be all caps, but it looks official and important that way.

Example Dialogue.json:
```
{
	"PROFESSIONAL": [
		{
			"Dialogue": [
				"words words words. also fart noises",
				"Ah, fellow mercs. Nothing personal, just business. You understand."
			],
			"BribeSuccessDialogue": [
				"On second thought, your money spends just as good as theirs. Initiating evac."
			],
			"BribeFailureDialogue": [
				"A contract is a contract, and we don't break ours."
			],
			"MinTimesEncountered": 0,
			"MaxTimesEncountered": -1,
			"BribeAcceptanceMultiplier": 1.0
		},
		{
			"Dialogue": [
				"Oh good, it's {Commander.Callsign} and their band of merry assholes {COMPANY.CompanyName}. We lost some good people and a better payday on {RCNT_SYSTEM} thanks to you. Time for some revenge."
			],
			"BribeSuccessDialogue": [
				"You're an asshole, but I'd rather have money than revenge. For now."
			],
			"BribeFailureDialogue": [
				"Oh piss off."
			],
			"MinTimesEncountered": 1,
			"MaxTimesEncountered": -1,
			"BribeAcceptanceMultiplier": 0.1
		}
	],
	"PIRATE": [
		{
			"Dialogue": [
				"AHOY SHITBIRDS"
			],
			"BribeSuccessDialogue": [
				"You're an asshole, but I'd rather have money than revenge. For now."
			],
			"BribeFailureDialogue": [
				"Oh piss off."
			],
			"MinTimesEncountered": 0,
			"MaxTimesEncountered": -1,
			"BribeAcceptanceMultiplier": 1.0
		}
	]
}
```

`Dialogue` - list of strings from which contract start dialogue will be randomly chosen. This dialogue will be interpolated using the same magic as event text so `{COMPANY.CompanyName}` will be replaced with the actual company name, etc. In addition, `{RCNT_SYSTEM}` will be replaced with the most recent star system where you faced this merc faction. Don't fuck up and put `{RCNT_SYSTEM}` in a dialogue bucket that doesn't have `MinTimesEncountered` > 0 because it'll just return an empty string.






So how does it all work together? We'll use the above settings as a guide. The only merc faction configured is the Kell Hounds. `BaseReplaceChance` in OpforReplacementConfig is 0, so they'll never replace the entire opfor, but `BaseReplaceChance` in `MercLanceAdditionConfig`