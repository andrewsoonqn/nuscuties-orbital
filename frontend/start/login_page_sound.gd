extends VBoxContainer

func _ready() -> void:
	pass
	
func _process(delta: float) -> void:
	pass

func _on_button_pressed() -> void:
	$ClickSoundEffect.play()
