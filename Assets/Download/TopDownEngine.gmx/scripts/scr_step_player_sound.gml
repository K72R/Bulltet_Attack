sound_wet = choose(sound_step_w1,sound_step_w2,sound_step_w3,sound_step_w4)
sound_dir = choose(sound_step_d1,sound_step_d2,sound_step_d3,sound_step_d4)
sound_con = choose(sound_step_c1,sound_step_c2,sound_step_c3,sound_step_c4)
sound_gra = choose(sound_step_g1,sound_step_g2,sound_step_g3,sound_step_g4)

var sound_step;

if o_terrain.rain = true
sound_step = sound_wet;
else if place_meeting(x,y,o_dirt)
sound_step = sound_dir;
else if tile_layer_find(1000000,x,y)
sound_step = sound_con;
else
sound_step = sound_gra;

    if (move_c > 0 and move_c <= 0+12)
    or (move_c > 180 and move_c <= 180+12)
    {
        sound_play(sound_step)
    }
   

