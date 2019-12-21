Feature: PlayerReference
	Does a bunch of tests on PlayerReferences

@PlayerReferenceTag
Scenario: Cannot add player reference with same name as an already existing player reference no matter letter casing
	Given a tournament named "GSL 2019" with player references "Maru, Stork" added to it
	When a player named "maRu" has been added to created group 0
	Then fetching player references from created tournament 0 should yeild 2 player references named: "Maru, Stork"
	
Scenario: Cannot rename player reference to same name as an already existing player reference no matter letter casing
	Given a tournament named "GSL 2019" with player references "Maru, Stork" added to it
	When renaming created player reference 0 to "maRu"
	Then fetching player references from created tournament 0 should yeild 2 player references named: "Maru, Stork"
