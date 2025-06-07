package database

import (
	"time"
)

type mockDB struct{}

var mockLoginDetails = map[string]LoginDetails{
	"alex": {
		AuthToken: "123ABC",
		Username:  "alex",
	},
	"jason": {
		AuthToken: "456DEF",
		Username:  "jason",
	},
	"marie": {
		AuthToken: "789GHI",
		Username:  "marie",
	},
}

var mockExpDetails = map[string]ExpDetails{
	"alex": {
		Exp:      100,
		Username: "alex",
	},
	"jason": {
		Exp:      200,
		Username: "jason",
	},
	"marie": {
		Exp:      300,
		Username: "marie",
	},
}

func (d *mockDB) GetUserLoginDetails(username string) *LoginDetails {
	// Simulate DB call
	time.Sleep(time.Second * 1)

	var clientData LoginDetails
	clientData, ok := mockLoginDetails[username]
	if !ok {
		return nil
	}

	return &clientData
}

func (d *mockDB) GetUserExp(username string) *ExpDetails {
	// Simulate DB call
	time.Sleep(time.Second * 1)

	var clientData ExpDetails
	clientData, ok := mockExpDetails[username]
	if !ok {
		return nil
	}

	return &clientData
}

func (d *mockDB) UpdateUserExp(username string, exp int) error {
	// Simulate DB call
	time.Sleep(time.Second * 1)

	var updatedData = ExpDetails{
		Username: username,
		Exp:      exp,
	}
	mockExpDetails[username] = updatedData

	return nil
}

func (d *mockDB) AddNewUser(username string) error {
	// Simulate DB call
	time.Sleep(time.Second * 1)

	var updatedData = ExpDetails{
		Username: username,
		Exp:      0,
	}
	mockExpDetails[username] = updatedData

	return nil
}

func (d *mockDB) SetupDatabase() error {
	return nil
}
