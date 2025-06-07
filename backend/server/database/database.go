package database

import (
	log "github.com/sirupsen/logrus"
)

type Database interface {
	GetUserLoginDetails(username string) *LoginDetails
	GetUserExp(username string) *ExpDetails
	UpdateUserExp(username string, exp int) error
	AddNewUser(username string) error
	SetupDatabase() error
}

type LoginDetails struct {
	AuthToken string
	Username  string
}

type ExpDetails struct {
	Exp      int
	Username string
}

func NewDatabase() (*Database, error) {
	var database Database = &mockDB{}

	var err = database.SetupDatabase()
	if err != nil {
		log.Error(err)
		return nil, err
	}

	return &database, nil
}
