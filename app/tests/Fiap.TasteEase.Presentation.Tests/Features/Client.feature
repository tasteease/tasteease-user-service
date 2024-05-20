Feature: ClientController

    In order to create a client
    As a user
    I want to be able to send a POST request to create a client

Scenario: Create client successfully
    Given a valid create client request
    When the POST request is sent to create a client to 'client'
    Then a 201 Created response is returned
    And the response contains the client ID
