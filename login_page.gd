extends VBoxContainer


func _on_button_pressed():
	get_tree().change_scene_to_file("res://home_page/main_menu.tscn")
