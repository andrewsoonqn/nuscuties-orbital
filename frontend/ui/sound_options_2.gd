extends MarginContainer

func _ready():
	var saved_value = ProjectSettings.get_setting("user_settings/sfx_volume", 100.0)
	$VBoxContainer/HSlider2.value = saved_value
	_on_h_slider_2_value_changed(saved_value)
	
	var saved_value_2 = ProjectSettings.get_setting("user_settings/bgm_volume", 100.0)
	$VBoxContainer/HSlider.value = saved_value_2
	_on_h_slider_value_changed(saved_value_2)


func _on_h_slider_2_value_changed(value: float) -> void:
	var db = lerp(-80, 0, value / 100.0)  # 0–100 slider mapped to -80dB to 0dB
	var sfx_bus_index = AudioServer.get_bus_index("SFX")
	AudioServer.set_bus_volume_db(sfx_bus_index, db)
	
	ProjectSettings.set_setting("user_settings/sfx_volume", value)
	ProjectSettings.save()


func _on_h_slider_value_changed(value: float) -> void:
	var db = lerp(-80, 0, value / 100.0)  # 0–100 slider mapped to -80dB to 0dB
	var bgm_bus_index = AudioServer.get_bus_index("BackgroundMusic")
	AudioServer.set_bus_volume_db(bgm_bus_index, db)
	
	ProjectSettings.set_setting("user_settings/bgm_volume", value)
	ProjectSettings.save()
