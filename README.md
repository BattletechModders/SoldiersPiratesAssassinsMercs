# SoldiersPiratesAssassinsMercs

**Depends On MissionControl and IRBTModUtils!** 

This mod breathes a little life into the mercenaries of the Inner Sphere, allowing mercenary factions to jump in and replace the opfor at contract start. Depending on the configuration, the hostile mercs may replace the opfor entirely, or they may only replace AdditionalLances added by MissionControl. The lance difficulty *should* be approximately the same, as the mod works by simply changing the faction for the replaced units and re-selecting appropriate lances and units. In addition, merc dialogue will be generated at contract start, and the capability to bribe/pay off the hostile mercs (only for MC AdditionalLances. No paying your way out of entire contracts) has also been implemented. Settings for this mod reside in two places; basic mod settings are in the mod.json, while Dialogue.json contains settings and configuration for dialogue (more on that later).

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
		"AlternateFactionConfigs": {
			"Liao": {
				"FactionReplaceChance": 1.0,
				"AlternateOpforWeights": {
					"KellHounds": 5,
					"Mercenaries": 10
				}
			}
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

`enableDebug` and `enableTrace` - bools, enable debug and trace logging, respectively. recommend leaving debug log disabled and trace enabled (at least for initial config)

`OpforReplacementConfig` and `MercLanceAdditionConfig` - configs for whole-opfor and MC AdditionalLances faction replacement, respectively.

* `BaseReplaceChance` - baseline chance for any opfor to be replaced by a valid merc faction. uses 0 - 1 range; 0 is never, 1 is guaranteed.
* `FactionsReplaceOverrides` - list of faction-specific overrides for the replacement chance. i.e, override Locals and Wobbies to hire mercs more often and override clans to never use them. uses 0 - 1 range; 0 is never, 1 is guaranteed.
* `BlacklistContractTypes` and `BlacklistContractIDs` - lists of contract types and specific contract IDs which will never replace opfor with mercs.
* `MercFactionReputationFactor` (in MercLanceAdditionConfig only) - custom reputation loss multiplier for hostile merc faction when they only replaced MC AdditionalLances (rep loss for original target faction is always maintained, and when whole opfor is replaced by mercs, both merc faction and contract target faction lose rep as normal). Mercenary faction reputation isn't hooked to anything besides regular rep system, but its there if you need it.

`AlternateFactionConfigs` - Dictionary of configs for potential alternate (non-merc!) factions that have a chance to replace the original target faction. Config things like 2nd line units, specific regiments, etc here. These factions *must* be real, valid factions. They have to have a FactionDef, and be defined in Faction.json. They do *not* have to be set to gain reputation, nor do they need to be IsCareerStartingDisplayFaction and display in the Cpt Quarters reputation screen. The KEYS for this dictionary are the faction Names for the configured alternate factions, i.e KellHounds (same field as Liao, Davion, Locals, etc). Using the above settings, Liao will always be replaced by a faction from AlternateOpforWeights due to FactionReplaceChance being 1.0. Given the above, Liao is twice as likely to be replaced by the generic Mercenaries faction as by the Kell Hounds.

* `FactionReplaceChance` - float, probability that original target faction will be replaced by one of these
* `AlternateOpforWeights` - dictionary <string, int> - faction Name and weight for different factions if replacement (from above) is happening
 
 `MercFactionConfigs` - Dictionary of configs for Merc Faction behavior. Merc factions *must* be real, valid factions. They have to have a FactionDef, and be defined in Faction.json. In particular, Faction.json must have them with IsRealFaction and IsMercenary set to true. They do *not* have to be set to gain reputation, nor do they need to be IsCareerStartingDisplayFaction and display in the Cpt Quarters reputation screen.
 
 The KEYS for this dictionary are the faction Names for the configured merc factions, i.e KellHounds (same field as Liao, Davion, Locals, etc). If a merc faction exists but is missing from this dictionary, it will not be used by SPAM!:
 
* `MercFactionName` - string, should be same as the KEY; the faction Name.
* `AppearanceWeight` - integer weight for this merc faction to be used for replacement (relative to other configured merc factions)
* `EmployerBlacklist` - list, strings; "employer" blacklist for merc faction. I.e KellHounds will never replace (be hired) by Liao.
* `UnitRating` - integer MRB unit rating for the merc faction. Calculates based on a 13 point system, as MRB ratings are given as an A+ through D- scale. We've added an F for NotRated. Merc Units that are NotRated get a 1, D- gets 2, A+ gets 13.
* `PersonalityAttributes` - list of PersonalityAttributes that will determine the "pool" of randomly generated dialogue pulled from the Dialogue.json

`FallbackUnitFactionTag` - fallback unit tag to use if the unit replacer cannot find a unit that matches the lance/unit selector tag for the chosen merc faction. Best to use your most "generic" mercenary faction tag, but de-capitalized. Really just to prevent fallback cicadas while hopefully still being merc-ey.

`BribeCostBaselineMulti` - baseline multiplier for calculation of bribes; multiplied by the total cost (battlevalue) of the mercs you're trying to bribe.


Dialogue.json consists of a Dictionary, where the KEYS are PersonalityAttributes you've decided to use for your merc factions. They can be literally whatever you want; there just needs to be a match between the PersonalityAttributes in mod.json and the KEYS in Dialogue.json. They also don't have to be all caps, but it looks official and important that way. Each VALUE is a list of something that I've decided to call a MercDialogueBucket. Because buckets are great.

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

`Dialogue` - list of strings from which contract start dialogue will be randomly chosen at random. This dialogue will be interpolated using the same magic as event text so `{COMPANY.CompanyName}` will be replaced with the actual company name, etc. In addition, `{RCNT_SYSTEM}` will be replaced with the most recent star system where you faced this merc faction. Don't fuck up and put `{RCNT_SYSTEM}` in a dialogue bucket that doesn't have `MinTimesEncountered` > 0 because it'll just return an empty string.

* `BribeSuccessDialogue` - list of strings from which dialogue is randomly chosen when a bribe attempt is successsful. Same interpolation as above.
* `BribeFailureDialogue` - list of strings from which dialogue is randomly chosen when a bribe attempt is successsful. Same interpolation as above.
* `MinTimesEncountered` and `MaxTimesEncountered` - Minimum and Maximum (duh) number of times you must have faced this merc faction in order for this dialogue bucket to be used.
* `BribeAcceptanceMultiplier` - float, multiplier for the likelihood that a bribe attempt will be successful, for a given bucket (so you can match the odds of bribe success to the "personality" of the merc faction in the dialogue).

If multiple dialogue buckets are usable according to the Min/Max TimesEncountered requirements, a bucket will be chosen at random (the bucket chosen at contract start also then supplies the success/failure dialogue for bribes, if applicable).

So how does it all work together? We'll use the above settings as a guide. 

### Are we going to replace something with mercs?

Well we're at least gonna try. `BaseReplaceChance` in OpforReplacementConfig is 0, so mercs will never replace the entire opfor, but `BaseReplaceChance` in `MercLanceAdditionConfig` is 1.0, so mercs will always replace MC AdditionalLances *unless* you're facing Locals, in which case they would not be replaced due to `FactionsReplaceOverrides`. We don't have any contracts or contract types blacklisted, so onwards.

### What mercs are we replacing them with?
The only merc faction configured is the Kell Hounds, so AppearanceWeight doesn't matter, but it would if we had multiple merc factions configured. For this example, it's gonna be KellHounds. Unless you're facing Liao, in which case nothing will happen.

### How does the actual replacement work?
Magic.gif mostly. Really just swaps out the target faction tags at unit selection; as long as the merc faction doesn't have any spawn holes in it for a given contract type, you should be fine (just like any other normal faction); in the event of a spawn hole we try to use `FallbackUnitFactionTag` ('mercenaries' in this case) to pick units.

### Neat. Now what?
Contract loads. At contract start, Dialogue is selected and displayed. First, all the Dialogue buckets from Dialogue.json are filtered according to the PersonalityAttributes for that Merc Faction. In the above example, the KellHounds only have PROFESSIONAL, so those are the only two dialogue buckets they get. If they also had PIRATE, all three dialogue buckets could be used. Available DialogueBuckets are then filtered by Min/Max TimesEncountered, and a final DialogueBucket is chosen from that list. After that, an actual string of dialogue is chosen at random from the `"Dialogue":[]` array within the bucket. That string is displayed at contract start along with all the other contract start dialogue (Darius going bla bla blah, EDM "it heckin wimdy", etc).

### Ok but what about bribes?
Bribes are available only if mercs replaced MC AdditionalLances, NOT when mercs replaced the entire opfor. So lets say its MC AdditionalLances. After at least a full round of combat has passed (sometimes 2 depending if you dropped immediately in combat or not), a generic popup will ask you if you want to try to bribe, and how much you'd like to spend. If a bribe is successful, the merc lance despawns (so no salvage), and the "destroy" objective for them should disappear. Or at least stop mattering. The popup will give you 5 options, with percentage ratings of 0%, 25%, 50%, 75%, and 100%. 0% is just "don't try to bribe."

### How much does a bribe cost?
The baseline cost of a bribe is determined by the total BattleValue (which in HBSBT is money) of the merc lance in question. This value is then multiplied by `BribeCostBaselineMulti` from mod.json to get the "full" cost of the bribe. Whichever % option you choose in the popup, then determines the % of the full cost you've decided to offer to the mercs. **You will always lose half the bribe cost upfront, even if the bribe is unsuccessfull.**

### How likely is the bribe to work?
The baseline chance of a bribe to be successful is determined by a couple things, starting at `BribeAcceptanceMultiplier` from the chosen DialogueBucket. Then we do some funass math with UnitRating (remember that from the MercFactionConfigs?), comparing it with the *players* MRB rating (which is converted to the same 1 - 13 scale as the merc UnitRating). We're then going to multiply the BribeAcceptanceMultiplier by `(player MRBRating / merc UnitRating)`. So a player with an extremely low MRB rating is probably going to have a hard time bribing a merc unit with a high UnitRating. This gives us our "final" chance of accepting the bribe, which is then factored via the % option you chose (same as cost).

### Cool, it worked?
The bribed merc units should despawn without appearing as salvage.

### Boo, it didn't worked?
Are you saying these mercenaries just *took* your money and didn't leave? Unbelievable. Guess you'll have to destroy them, huh.
