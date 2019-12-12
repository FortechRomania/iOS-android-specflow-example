Feature: As a user I want to modify an acquaintance

  Background: Preconditions
    Given I tap on the add acquaintance button
    And   I set the first name to: 'Andrew' and last name to: 'Boild' and tap the Save button

  Scenario: Delete newly created acquaintance

    Given I tap on the new acquaintance
    When  I tap on the delete acquaintance button and confirm my action
    Then  I should see that the acquaintance was deleted

  Scenario: Edit newly created acquaintance

    Given I tap on the new acquaintance
    And   I tap on the edit acquaintance button
    When  I set the first name to: 'Aleksandr' and last name to: 'Arlus' and tap the Save button
    Then  I should see that the first name and last name changed