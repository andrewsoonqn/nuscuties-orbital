extends LineEdit

var has_focused_once := false

func _ready():
	self.placeholder_text = "Username (max 15 characters)"

func _gui_input(event):
	if event is InputEventMouseButton and event.pressed and not has_focused_once:
		self.placeholder_text = ""
		has_focused_once = true


func _on_button_pressed() -> void:
	pass # Replace with function body.
