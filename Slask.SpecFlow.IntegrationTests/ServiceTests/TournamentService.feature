Feature: TournamentService
	Does a bunch of tests on TournamentService

@TournamentServiceTag
Scenario: Can create tournament
	#Given I have entered 50 into the calculator
	#And I have entered 70 into the calculator
	#When I press add
	#Then the result should be 120 on the screen
	Given a TournamentService has been created
	And a tournament named "GSL 2019" has been created
	Then tournament should be valid with name "GSL 2019"
	
Scenario: Cannot create tournament with empty name
	Given a TournamentService has been created
	And a tournament named "" has been created
	Then tournament should be null

#Scenario: Cannot create tournament with name already in use no matter letter casing
#	
#Scenario: Can rename tournament
#
#Scenario: Cannot rename tournament to empty name
#
#Scenario: Cannot rename tournament to name already in use no matter letter casing
#
#Scenario: Cannot rename non-existing tournament
#
#Scenario: Can get tournament by id
#
#Scenario: Can get tournament by name
#
#Scenario: Can add user to tournament with user service
#
#Scenario: Can only add user to tournament once
#
#Scenario: Can get all betters in tournament by tournament id
#
#Scenario: Can get all betters in tournament by tournament name
#
#Scenario: Player references are added to tournament when new players are added to tournament
#
#Scenario: Can get all player references in tournament by tournament id
#
#Scenario: Can get all player references in tournament by tournament name


