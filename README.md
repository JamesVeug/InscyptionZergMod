# Zerg Mod
This mod brings the alien race 'Zerg' from the RTS series StarCraft, into the world of Inscryption.

The Zerg Swarm is a terrifying and ruthless amalgamation of biologically advanced, arthropodal aliens. Dedicated to the pursuit of genetic perfection.

## Includes:
- 30 new Cards
- 16 new Sigils
- 3 new Special Abilities

### Cards:
- **Drone** - 1,1 with Bone digger and Submerge. Evolves into Crawler Forest
- **Zerglings** - 1,2 with Double Attack. Evolves into Baneling. Special: Zergling Swarm
- **Broodling** - 2,2 with Brittle.
- **Locust** - 1,1 with Brittle.
- **Infested Terran** - 1,2 with Brittle and Fecundity.
- **Overlord** - 0,4 with Sacrificial and Mighty Leap. Evolves into Overseer.
- **Roach** - 2,3 with Regenerate. Evolves into Ravager.
- **Mutalisk** - 1,3 with Ricochet and Regenerate. Evolves into Guardian
- **Queen** - 1,3 With Spawn Larva.
- **Hydralisk** - 3,2. Evolves into Lurker.
- **Scourge** - 3,1 with Explode and Airborne.
- **Corruptor** - 2,2 with Airborne. Evolves into Devourer.
- **Crawler Forest** - Mirror,5 with Mighty Leap and Detector
- **Egg** - 0,3
- **Baneling** - 3,1 with Explode
- **Overseer** - 0,6 with Detector and Stinky
- **Guardian** - 4,6
- **Swarm Host** - 0,2 with Spawn Locust and Waterborne
- **Larva** - 0,3 with Evolve. Special: Larva, Kills Survivors

### Rare Cards:
- **Brood lord** - 0,6 with Swarm Seeds
- **Infestor** - 1,1 with Waterborne and Fish Hook
- **Dehaka** - 1,2 with Regenerate. Special: Collect Essence
- **Leviathan** - 0,20 with Blood Bank and Summon Zerg
- **Queen of Blades** - 4,4 With Regestate
- **Viper** - 2,1 With Abduct
- **Devourer** - 3,3 with Airborne and Armored.
- **Ultralisk** - 2,5 with Armored and Splash Damage.
- **Ravager** - 2,2 with Snipe.
- **Lurker** - 2,2 with Waterborne and Splash Damage.
- **Strange Artifact** - 0,1 with Bone King and Worthy Sacrifice

### Sigils:
- **Swarm Seeds** - Draw 1 Broodling at the start of your turn
- **Spawn Locust** - Draw 1 Locust at the start of your turn
- **Regenerate** - The card bearing this sigil Heals 1 health at the end of a turn
- **Spawn Larva** - The card bearing this sigil will place 2 lava on the board
- **Double Attack** - When a card bearing this sigil deals damage to a card and survives, it will perform one additional attack.
- **Regestate** - When this card is killed it will transform into an Egg for it to regenerate into its original form. An egg has 0 Power and as much Health as the base card.
- **Armored** - When a card bearing this sigil takes damage, it will take 1 less damage
- **Splash Damage** - When a card bearing this sigil deals damage it will also hit the adjacent cards
- **Abduct** - When a card bearing this sigil is played, a targeted enemy card is moved to the space in front of it, if that space is empty
- **Detector** - When a card bearing this sigil is on the board all opponent's submerged cards will be revealed.
- **Explode** - When a card bearing this sigil deals damage it will also hit the adjacent cards and perish.
- **Bombard** - When a card bearing this sigil deals damage directly it will damage the opposing card for 1 damage.
- **Ricochet** - When a card bearing this sigil deals damage to a card it also hits face for 1 damage.
- **Fish Hook** - When a card bearing this sigil is played, a targeted card is moved to your side of the board.
- **Blood Bank** - When a card bearing this sigil is sacrified, its health is reduced relative to blood required.
- **Summon Zerg** - When a card bearing this sigil takes damage it will create a random zerg card in your hand

### Special Abilities:
- **Zergling Swarm** - Portrait changes as the health increases. Max 6
- **Collect Essence** - When a card with this sigil kills a stronger card it will steal its essence.
- **Larva** - When a Larva evolves it will transform into a random Zerg card


## Credits:
- **JamesGames** - Creator
- **CarbotAnimations** - Original Cartoon art for Star Craft
- **GeneralSpritz** - Custom Art
- **Shattered Omega** - Balancing suggestions
- **Cyantist** - Armored Ability


# Installation
For this mod you will need
https://inscryption.thunderstore.io/package/BepInEx/BepInExPack_Inscryption/ - BepInEx
https://inscryption.thunderstore.io/package/API_dev/API/ - API
I recommend this tutorial if you are curious how to manually install mods in the first place https://youtu.be/HgCtjtXraog


## Known Issues:
- Regestate does not work for death cards
- Armored does not work on some sigils


### Bug Reports / Suggestions:
Write an issue report here: https://github.com/JamesVeug/InscyptionZergMod/issues



# Update notes:

## `Version: 0.9.0 - 15/12/2021`
### New:
- Added Summon Zerg Ability
- Added Blood Bank Ability
- Added Larva

### Changes:
- General
  - Updated all Portraits with higher resolution art
- Leviathan
  - Reduced Power from 2 to 0
  - Increased Health from 10 to 20
  - Reduced Bone Cost from 10 to 8
  - Removed WhackAMole and Sharp Abilities
  - Added Blood Bank and Summon Zerg Abilities
- Mutalisk
  - Added Regenerate
- Strange Artifact
  - Can no longer Kill Survivors
- Squirrel
  - Reverted back to normal
- Overlord
  - Removed Fledgling
  - Added Sacrificial
  - Increased Blood from 1 to 2

### Fixes:
- Viper
  - Correct Emission
- Scourge
  - Fixed Portrait offset
- Ricochet
  - Fixed Ricochet damage not setting to 1



<details>
  <summary>See all Changes</summary>

## `Version: 0.8.0 - 5/1/2021`
### New:
- Strange Artifact

### Changes:
- General
  - Added first pass emissions to all cards except Zerglings
  - Removed rare background from cards that are not rare
- Ricochet
  - Can now be blocked by Mighty Leap
- Swarm Host
  - No longer rare. Obtainable from Trader
- Dehaka
  - Blood cost increased from 1 to 2
- Scourge
  - Damage increased to 3 from 1
  - Obtainable now

### Fixes:
- Dehaka
  - Fixed portrait not changing on boot
- Zerglings
  - Fixed portrait not changing when buffing hp at campfire
  - Fixed portrait not changing on boot
- Regestate
  - Fixed HP of egg not accounting for buffed health from fire.


## `Version: 0.7.0 - 2/12/2021`
### New:
- Crawler Forest

### Changes:
- General
  - Updated some Descriptions to be more descriptive
- Lurker
  - Removed Guard Dog Ability
  - Added Splash Damage Ability
- Drone
  - Added Submerge
  - Can now Evolve into Crawler Forest
- Double Attack
  - Can now double attack cards that are created after the initial cards death 
- Abduct
  - Can now be canceled by clicking on an empty slot
- Spawn Larva
  - Added new art by General Spritz
- Armoured
  - Renamed to Armored
- Draw Broodling's
  - Renamed to Swarm Seeds
- Draw Locust's
  - Renamed to Spawn Locust

### Fixes:
- Sometimes Abduct does not wiggle when it can not cast
- Soft lock when sacrificing a card that has the Detector sigil and has revealed submerged cards

## `Version: 0.6.0 - 28/11/2021`
### New:
- Added Collect Essence special ability
- Added Fish Hook ability
- Added Ricochet ability

### Changes:
- General
    - Updated some descriptions
- Dehaka
    - Added Collect Essence special ability
    - Portrait changes as he kills strong units
- Infestor
    - Removed Trifurcated Strike ability
    - Added Fish Hook ability
- Ultralisk
    - Now obtainable after defeating Prospector
- Mutalisk 
    - Health reverted back to 3 from 1
    - Removed Bombard ability 
    - Removed Airborne ability
    - Added Ricochet ability
- Roach
	- Health increased from 2 to 3
- Armoured ability
	- Added new art by General Spritz

### Fixes:
- Splash damage sometimes doesn't hit a
- Fixed Bombard hitting facedown cards



## `Version: 0.5.0 - 26/11/2021`
### New:
- Added Bombard ability

### Changes:
- General
  - Rebalanced drop rates of rare cards
  - Updated some descriptions
- Mutalisk
  - Health Reduced from 3 to 1
  - Regenerate ability removed
  - Bombard ability added
- Infested Terran
  - Bone cost increase from 1 to 2
- Draw Broodling's
  - Reduced cards drawn from 2 to 1
- Draw Locust's
  - Reduced cards drawn from 2 to 1
- Regestate
  - Health of egg now the same as the card it evolves into
  - Total turns to evolve now depends on health of card evolving into
- Double Attack
  - No longer hits face
    - Halved animation
- Devourer
  - Is now Rare
- Guardian
  - Is now Rare
- Ravager
  - Is now Rare
- Lurker
  - Is now Rare
  - Accessible from boss fights

### Fixes:
- Rare cards obtainable from common card map node 
- Splash damage no longer hits face down cards
- Fixed Regestate not having the Fledgling sigil to evolve the egg



## `Version: 0.4.0 - 24/11/2021` 
### New:
- Added Viper - 2,1 with Abduct
- Added Scourge - 1,1 with Explode and Airborne
- Added Corruptor - 2,2 with Airborne. Evolves into Devourer
- Added Devourer - 3,3 with Airborne and Armoured
- Added Guardian - 4,6

- Added Abduct Ability
- Added Detector Ability
- Added Explode Ability

### Changes:
- Infested Terran
	- Removed Blood cost so now its just 1 Bone
- Ravager
	- Renamed from Ravanger to Ravager
- Lurker
	- Removed Burrower ability
	- Added GuardDog ability
- Overlord
	- Removed Airborne ability
	- Added Fledgling ability
- Overseer
	- Removed Reach and Airborne abilities
	- Added Detector and Stinky abilities
- Splash Damage
	- Added new art by General Spritz
- Double Attack
    - No longer hits face

### Fixes:
- Fixed Infested Terran costing 1 Blood and 1 Bone

## `Version: 0.3.0 - 23/11/2021`
### New:
- Added Ravenger - 2,2 with Snipe
- Added Lurker - 2,2 with Submerge and Burrower

- Added Armoured Ability
- Added Splash Ability

- Added Watermark to all Zerg cards

### Changes:
- Bumped API plugin requirement to 1.12.0
- Hydralisk
	- Evolves into a Lurker
- Roach 
	- Evolves into a Ravenger
- Broodlord
	- Health changed to 6 from 4
	- Blood changed to 3 from 2
- Broodling
	- Damage changed to 2 from 1
	- Health changed to 2 from 1
- Queen of Blades 
	- Damage changed to 4 from 3
	- Health changed to 4 from 3
- Ultralisk 
	- No longer accessible from Trader
	- Removed Deathshield ability
	- Added Armoured ability
	- Added Splash Damage ability
- Baneling
	- Removed Trifurcated Strike ability
	- Added Splash Damage ability


## `Version: 0.2.0 - 22/11/2021`
### New:
- Added Drone - 1,1 with Bone digger
- Added Baneling - Rare - 3,1 with Brittle and Trifurcated Strike
- Added Swam Host - Rare - 0,2 with Draw 2 Locusts and Submerge
- Added Locus - 1,1 with Brittle
- Added Broodlord - 0,4 with Draw 2 Broodlings
- Added Bloodling - 1,1 with Brittle
- Added Infested Terran - 1,2 with Brittle and Fecundity
- Added Overseer - 0,6 - Rare with Reach and Airborne
- Added Infestor - 1,1 - Rare with Submerge and Trifurcated Strike
- Added Dehaka - 1,2 - Rare with Regenerate
- Added Leviathan - 2,10 - with Burrower and Sharp
- Added Ultralisk - 2,6 - with Death Shield

- Added Draw Broodlings ability
- Added Draw Locusts ability

### Changes:
- Overlord 
	- Evolves into Overseer
- Zerglings 
	- Evolve into Banelings
	- Portrait now shows how many zerglings as health. Max 6.
- Queen of Blades
	- No longer obtainable from Trader
- Mutalisk
	- Removed Trifurcated Strike
	- Added Regeneration
	- Damage changed to 1 from 2
	- Health changed to 3 from 1
- Double Attack
	- Added new art by General Spritz
- Regeneration
	- Added new art by General Spritz

### Fixes:
- Fixed all zerg card backgrounds showing as Rare



## `Version: 0.1.1 - 19/11/2021`
### New:
- All Zerg cards
    - Now have the Insect Trait
### Changes:
- Mutalisk 
	-	Damage changed to 2 from 3
- Larva 
	- Health reduced back to 1
- Kerrigan 
	- Renamed to Queen of Blades
- Final attack 
	- Renamed to Double Attack
- Touched up some card descriptions
- Touched up some ability dialogue

### Fixes:
- Fixed mod not working due to wrong directory
- Fixed Final attack direct attack animation
- Fixed Final attack sometimes not attacking


## `Version: 0.1.0 - 17/11/2021`
### New:
- Added Zergling - 1,2 with Final attack
- Added Roach - 2,2 with Regenerate
- Added Overlord - 0,4 with Airborne and Mighty Leap
- Added Kerrigan - 3,3 With Regestate
- Added Mutalisk - 3,1 with Airborne and Bifurcated Strike
- Added Queen - 1,3 With Spawn Larva
- Added Hydralisk - 3,2

- Added Regenerate ability
- Added Spawn Larva ability
- Added Final attack ability
- Added Regestate ability

### Changes:
- Squirrel art changed to a Larva

</details>