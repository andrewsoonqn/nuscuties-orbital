shader_type canvas_item;

uniform sampler2D texture_albedo : hint_albedo;

void fragment() {
    vec2 uv = UV - vec2(0.5);  // Center UV
    float r = length(uv);
    if (r > 0.5) {
        discard;  // Make outside transparent
    }
    COLOR = texture(texture_albedo, UV + vec2(0.5));
}
