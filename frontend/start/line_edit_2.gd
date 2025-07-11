extends LineEdit

var has_focused_once := false

func _ready():
	self.placeholder_text = "Password"

func _gui_input(event):
	if event is InputEventMouseButton and event.pressed and not has_focused_once:
		self.placeholder_text = ""
		has_focused_once = true
