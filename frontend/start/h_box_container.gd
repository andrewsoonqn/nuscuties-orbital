extends HBoxContainer

func _on_LineEdit_focus_entered():
	$LineEdit.placeholder_text = ""
