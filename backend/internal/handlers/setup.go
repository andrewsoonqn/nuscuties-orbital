package handlers

import (
	"github.com/go-chi/chi"
	"github.com/go-chi/chi/middleware"
)

func Handler(r *chi.Mux) {
	r.Use(middleware.StripSlashes)

	r.Route("/users", func(router chi.Router) {
		router.Get("/{username}/exp", GetExp)
		router.Put("/{username}/exp", PutExp)
		router.Post("/register", RegisterUser)

	})
}
