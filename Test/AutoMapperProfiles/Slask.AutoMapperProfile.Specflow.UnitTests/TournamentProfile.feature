Feature: TournamentProfile
	Making sure the tournament profile maps tournaments correctly with auto-mapper

@TournamentProfileTag
Scenario: Can map a domain tournament to a tournament DTO
	Given a tournament named "GSL 2020" has been created with users "Stålberto, Bönis, Guggelito, Kimmieboi" added to it
		And tournament named "GSL 2020" adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament named "GSL 2020"
		And betters places match bets in tournament named "GSL 2020"
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 0           | 0           | 0           | Maru        |
			| Stålberto   | 0           | 0           | 1           | Taeja       |
			| Bönis       | 0           | 0           | 0           | Stork       |
			| Bönis       | 0           | 0           | 1           | Rain        |
			| Guggelito   | 0           | 0           | 0           | Stork       |
			| Guggelito   | 0           | 0           | 1           | Taeja       |
			| Kimmieboi   | 0           | 0           | 0           | Stork       |
			| Kimmieboi   | 0           | 0           | 1           | Rain        |
		And score is added to players in given matches within groups in tournament named "GSL 2020"
			| Round index | Group index | Match index | Scoring player | Score Added |
			| 0           | 0           | 0           | Maru           | 2           |
			| 0           | 0           | 1           | Taeja          | 2           |
		And betters places match bets in tournament named "GSL 2020"
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 0           | 0           | 2           | Taeja       |
			| Bönis       | 0           | 0           | 2           | Maru        |
			| Guggelito   | 0           | 0           | 2           | Taeja       |
			| Kimmieboi   | 0           | 0           | 2           | Taeja       |
		And score is added to players in given matches within groups in tournament named "GSL 2020"
			| Round index | Group index | Match index | Scoring player | Score Added |
			| 0           | 0           | 2           | Taeja          | 2           |
	When automapper maps domain tournament "GSL 2020" to a tournament DTO
	Then automapped tournament DTO named "GSL 2020" should be valid with
			| Better count | Round count | Issue count |
			| 4            | 1           | 0           |
		And automapped tournament DTO named "GSL 2020" should contain betters "Stålberto, Bönis, Guggelito, Kimmieboi"
		And automapped tournament DTO named "GSL 2020" should contain rounds
			| Contest type | Round name |
			| Bracket      | Round A    |
		And automapped tournament DTO named "GSL 2020" should contain groups
			| Round index | Group index | Contest type | Sort order | Group name |
			| 0           | 0           | Bracket      | 0          | Group A    |
		And automapped tournament DTO named "GSL 2020" should contain matches
			| Round index | Group index | Match index | Sort order | Best of | Player1 name | Player1 score | Player2 name | Player2 score |
			| 0           | 0           | 0           | 0          | 3       | Maru         | 2             | Stork        | 0             |
			| 0           | 0           | 1           | 1          | 3       | Taeja        | 2             | Rain         | 0             |
			| 0           | 0           | 2           | 2          | 3       | Maru         | 0             | Taeja        | 2             |
		And automapped tournament DTO named "GSL 2020" should contain no issues
		And automapped tournament DTO named "GSL 2020" should contain better standings
			| Better name | Points |
			| Stålberto   | 3      |
			| Guggelito   | 2      |
			| Kimmieboi   | 1      |
			| Bönis       | 0      |
		
Scenario: Mapped tournament DTO is filled with tournament issues when they are present
	Given a tournament named "GSL 2020" has been created
		And tournament named "GSL 2020" adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 4                       |
			| Bracket      | 1                         | 4                       |
	When automapper maps domain tournament "GSL 2020" to a tournament DTO
	Then automapped tournament DTO named "GSL 2020" should contain issues
		| Round | Group | Match | Description                                                                                                                                        |
		| 0     | -1    | -1    | Current player count does not fill all group(s) to capacity. Add more players or reduce group capacity.                                            |
		| 1     | -1    | -1    | Round does not synergize with previous round. Advancing players from previous round will not fill the groups within the current round to capacity. |