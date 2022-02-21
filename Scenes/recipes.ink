VAR inventory_0 = ""
VAR inventory_1 = ""
VAR inventory_2 = ""
VAR rent_remaining = 200

VAR berries_and_cream_value = 11
VAR creamy_pie_value = 21
VAR potato_pie_value = 28
VAR cheesey_broccoli_value = 31
VAR spicy_broccoli_value = 34
VAR gentle_thigh_and_chips_value = 85
VAR creamy_gentlebird_pie_value = 75
VAR berry_pie_value = 39
VAR quesadilla_value = 42
VAR fish_taco_value = 45
VAR fish_and_chips_value = 32
VAR chips_value = 16
VAR sweet_potato_fries_value = 30
VAR sweet_potato_value = 11
VAR fish_pie_value = 52
VAR cake_value = 45
VAR mashed_potatoes_value = 24
VAR bread_value = 18
VAR sugar_fly_fries_value = 69
VAR extra_spicy_quesadilla_value = 54
VAR pepper_popper_value = 36
    
-> holding_knot
== holding_knot ==
    - you shouldn't read this ever # holding_knot
    -> DONE
    
== continue_to_holding ==
    + \(continue\)
        -> holding_knot

== function inventory_has(item) ==
{
    - inventory_0 == item:
        ~ return true
    - inventory_1 == item:
        ~ return true
    - inventory_2 == item:
       ~ return true
}
    ~ return false

// opening
== introduction ==
    - You wake up to a terrible shock! Someone has broken into your kitchen-apartment! They've stolen all of your money and your ingredients and it looks they tore up your recipe book!"
    + "Dang, what now?"
    - As the Royal Chef, maybe you can talk to your employer/landlord, King Cake. He likes to hang out in the next room over. But before you go, you gotta learn the controls!
    + "The controls???"
    - Yeah! Use WASD to move and press SPACE to talk to people or read pages and signs. SPACE also lets you cook at your oven over there to the West! You can pick up some items with E and drop them with Q.
    + "Sounds intuitive!"
    - Kind of! Good luck talking to King Cake! I'm sure he'll understand...
    + "Awesome, this is gonna be fine"
    -> continue_to_holding

// npcs
== king_cake ==
    - {rent_remaining <= 0: You've paid off your rent and even fed me an additional ${-1 * rent_remaining} of food. Congratulations my boy! Now I must reveal my terrible secret! | {king_cake_intro: "Tenant! I must insist you pay your remaining ${rent_remaining} rent or feed me items of equivalent value!" | You approach the domineering King Cake}}
    + {!king_cake_intro} "My Liege!"
        -> king_cake_intro
    + {rent_remaining <= 0} "Secret..?"
        -> king_cake_outro
    + {inventory_has("berriesAndCream")} "Try these Berries and Cream"
      ~ rent_remaining = rent_remaining - berries_and_cream_value
      "Dang that was delicious. Have ${berries_and_cream_value}" # take_berriesAndCream
        -> continue_to_holding
        
    + {inventory_has("creamyPie")} "Try this Creamy Pie" 
    ~ rent_remaining = rent_remaining - creamy_pie_value 
    "Ah a lovely creamy pie indeed. You know I always wanted to taste a creamy Gentlebird pie made from a rare species of sparrow, quite the exotic dish. You know I heard one lives up North and being the great chef you are I think you're up to the task of making one for me. Have ${creamy_pie_value}" # take_creamyPie 
    -> continue_to_holding
    
    
    + {inventory_has("potatoPie")} "Try this Creamy Potato Pie" 
    ~ rent_remaining = rent_remaining - potato_pie_value 
    "Ah a lovely creamy pie indeed. You know I always wanted to taste a creamy Gentlebird pie made from a rare species of sparrow, quite the exotic dish. You know I heard one lives up North and being the great chef you are I think you're up to the task of making one for me. Have ${potato_pie_value}" # take_potatoPie 
    -> continue_to_holding
    
    
    + {inventory_has("cheeseyBroccoli")} "Try this Cheesey Broccoli" 
    ~ rent_remaining = rent_remaining - cheesey_broccoli_value 
    "Dang that was delicious. Have ${cheesey_broccoli_value}" # take_cheeseyBroccoli 
    -> continue_to_holding
    
    
    + {inventory_has("spicyBroccoli")} "Try this Spicy Broccoli" 
    ~ rent_remaining = rent_remaining - spicy_broccoli_value 
    "Dang that was delicious. Have ${spicy_broccoli_value}" # take_spicyBroccoli 
    -> continue_to_holding
    
    
    + {inventory_has("gentleThighAndChips")} "Try this Gentle Thigh and Chips" 
    ~ rent_remaining = rent_remaining - gentle_thigh_and_chips_value 
    "Well, I have to admit kind of bland and has a horrible after taste, Gentle Thigh and Chips is not food fit for a grand king with a grand taste like mine but it is worth a lot on the free market so you still get a big pay for it, chef. Have ${gentle_thigh_and_chips_value}" # take_gentleThighAndChips 
    -> continue_to_holding
    
    
    + {inventory_has("creamyGentlebirdPie")} "Try this Creamy Gentlebird Pie" 
    ~ rent_remaining = rent_remaining - creamy_gentlebird_pie_value 
    "Well, I have to admit kind of bland and has a horrible after taste, Creamy Gentlebird Pie is not food fit for a grand king with a grand taste like mine but it is worth a lot on the free market so you still get a big pay for it, chef. Have ${creamy_gentlebird_pie_value}" # take_creamyGentlebirdPie 
    -> continue_to_holding
    
    
    + {inventory_has("berryPie")} "Try this Creamy Berry Pie" 
    ~ rent_remaining = rent_remaining - berry_pie_value 
    "Ah a lovely creamy pie indeed. You know I always wanted to taste a creamy Gentlebird pie made from a rare species of sparrow, quite the exotic dish. You know I heard one lives up North and being the great chef you are I think you're up to the task of making one for me. Have ${berry_pie_value}" # take_berryPie 
    -> continue_to_holding
    
    
    + {inventory_has("quesadilla")} "Try this Quesadilla" 
    ~ rent_remaining = rent_remaining - quesadilla_value 
    "Dang that was delicious. Have ${quesadilla_value}" # take_quesadilla 
    -> continue_to_holding
    
    
    + {inventory_has("fishTaco")} "Try this Fish Taco" 
    ~ rent_remaining = rent_remaining - fish_taco_value 
    "Dang that was delicious. Have ${fish_taco_value}" # take_fishTaco 
    -> continue_to_holding
    
    
    + {inventory_has("fishAndChips")} "Try this Fish and Chips" 
    ~ rent_remaining = rent_remaining - fish_and_chips_value 
    "Dang that was delicious. Have ${fish_and_chips_value}" # take_fishAndChips 
    -> continue_to_holding
    
    
    + {inventory_has("chips")} "Try these Chips" 
    ~ rent_remaining = rent_remaining - chips_value 
    "Dang that was delicious. Have ${chips_value}" # take_chips 
    -> continue_to_holding
    
    
    + {inventory_has("sweetPotatoFries")} "Try this Sweet Potato Fries" 
    ~ rent_remaining = rent_remaining - sweet_potato_fries_value 
    "Dang that was delicious. Have ${sweet_potato_fries_value}" # take_sweetPotatoFries 
    -> continue_to_holding
    
    
    + {inventory_has("fishPie")} "Try this Creamy Fish Pie" 
    ~ rent_remaining = rent_remaining - fish_pie_value 
    "Ah a lovely creamy pie indeed. You know I always wanted to taste a creamy Gentlebird pie made from a rare species of sparrow, quite the exotic dish. You know I heard one lives up North and being the great chef you are I think you're up to the task of making one for me. Have ${fish_pie_value}" # take_fishPie 
    -> continue_to_holding
    
    
    + {inventory_has("cake")} "Try this Cake" 
    ~ rent_remaining = rent_remaining - cake_value 
    "A cake! How dare you, I am a great king, I am beyond such tribal cannibali-mmm you did the icing really well, very sugary. Good stuff, chef! Have ${cake_value}" # take_cake 
    -> continue_to_holding
    
    
    + {inventory_has("mashedPotatoes")} "Try this Mashed Potatoes" 
    ~ rent_remaining = rent_remaining - mashed_potatoes_value 
    "Dang that was delicious. Have ${mashed_potatoes_value}" # take_mashedPotatoes 
    -> continue_to_holding
    
    
    + {inventory_has("bread")} "Try this Bread" 
    ~ rent_remaining = rent_remaining - bread_value 
    "Dang that was delicious. Have ${bread_value}" # take_bread 
    -> continue_to_holding
    
    
    + {inventory_has("sugarFlyFries")} "Try this Sugar Fly Fries" 
    ~ rent_remaining = rent_remaining - sugar_fly_fries_value 
    "Dang that was delicious. Have ${sugar_fly_fries_value}" # take_sugarFlyFries 
    -> continue_to_holding
    
    
    + {inventory_has("extraSpicyQuesadilla")} "Try this Extra Spicy Quesadilla" 
    ~ rent_remaining = rent_remaining - extra_spicy_quesadilla_value 
    "Dang that was delicious. Have ${extra_spicy_quesadilla_value}" # take_extraSpicyQuesadilla 
    -> continue_to_holding
    
    
    + {inventory_has("pepperPopper")} "Try this Pepper Popper" 
    ~ rent_remaining = rent_remaining - pepper_popper_value 
    "Dang that was delicious. Have ${pepper_popper_value}" # take_pepperPopper 
    -> continue_to_holding
    
    + {king_cake_intro} "I take my leave, Your majesty!"
       King Cake nods curtly
        -> continue_to_holding
    
== king_cake_intro ==
    - "Subject! It has reached mine royal ears that a terrible crime has been committed against you! Forsooth!"
    + "Yeah, someone stole all my money and ingredients and even wrecked my recipe book."
    - "'SWOUNDS! As you loving ruler, I extend my sincerest condolences, but as your landlord, I must remind you that the rent is due tomorrow!"
    + "Oh jeez -- can I get an extension in light of the situation?"
    - "NO! If you wish to keep living in my kitchen, you'll need to pay the rent in full!"
    + "But I don't have any money..."
    - "HMMMMMMMMMMMM. Since these are extenuating circumstances, just this time We're willing to bend the rules. Instead of the rent, you may simply feed me food whose cumulative retail value equals the required sum"
    + "...But I don't have any ingredients and my recipe book is ruined"
    - "Oh, just go find some ingredients and stuff in the world. You're the kingdom's greatest chef after all, I'm sure you'll think of something. Now off with you!"
    -> continue_to_holding
    
== king_cake_outro ==
  - "My treasured subject, it was I! I was the one who stole your ingredients and all of your money and, yes, even ruined your recipe book!"
  + "But why?"
  - "To test you, m'boy! You showed real canny out there today! Now I know for sure that you are indeed the greatest chef in all the kingdom."
  + "Can I have my money and ingredients back?"
  - "NO!"
  + "Dang"
  - <<THE END>>
  -> continue_to_holding
  
 == gentle_bird ==
    - {gentle_bird_quest_complete: "Come on, pick me up and carry me somewhere interesting" |{gentle_bird_intro: "So what if the clouds are bloody big garlic slices. Mmmmmmm. Oh hello mate, caught me stuck in thought. So you got something to light my cigar with yet?" | "So hey mate, I was just sitting here thinking about life..."}}
    + {!gentle_bird_intro} "Uh huh"
       -> gentle_bird_intro
    + {gentle_bird_intro && !inventory_has("pepper")} "Sorry, not yet"
        -> holding_knot
    + {gentle_bird_intro && inventory_has("pepper")} "I have this firey Pepper. It's literally on fire!"
        -> gentle_bird_quest_complete
    + {gentle_bird_quest_complete} "You got it"
        -> holding_knot

== gentle_bird_intro ==
    - "...and like, ye know, I was thinking for a long time and --oh you won't believe this buddy-- then i realized after 3 hours of thinking, my cigar! I'd forgot to light it up."
    + "Oh"
    - "But mate you won't believe the next part, I'd also forgotten the lighter and like I can't go all the way back to get it especially when I'm in big bloody thinking mode. So mate could I ask you a favor?"
    + "What do you need?"
    - "Could you please go out and find something to light this cigar with? Thank you, sweetie."
    -> continue_to_holding

== gentle_bird_quest_complete ==
    - "Ah so now you've lit my cigar mate, mind just being the jolly ol' taxi and dropping me off somewhere? You can just pick me up." # itemize
    -> continue_to_holding

== watering_hole ==
  - There's a little oasis here. Looks like something's swimming in there.
  + {inventory_has("fishingPole")} Catch the fish
    -> watering_hole_quest_complete
  + "Neat!"
    -> holding_knot
    
== watering_hole_quest_complete ==
    - Dang, you broke your fishing pole, but at least you did catch one fish! # take_fishingPole # give_fish
    -> continue_to_holding

// item descriptions
== crossroads_sign ==
    - North: Mingus Farms Creamery
    - East: Big Looping Desert
    - South: King Cake's Cool Castle
    - West: Underpopulated Broccoli Forest
    -> continue_to_holding

== berries ==
    - A handful of delicious red berries
    -> continue_to_holding

== berries_and_cream_recipe ==
  - Berries and Cream -- Retail Value ${berries_and_cream_value}:
  - \- 1 Berries
  - \- 1 Cream
  -> continue_to_holding
  
== fish_and_chips_recipe ==
  - Fish and Chips -- Retail Value ${fish_and_chips_value}:
  - \- 1 Fish
  - \- 1 Chips
  -> continue_to_holding
  
== sweet_potato_fries_recipe ==
  - Sweet Potato Fries -- Retail Value ${sweet_potato_fries_value}:
  - \- 1 Sweet Potato
  - \- 1 Oil
  -> continue_to_holding


== potato_pie_recipe ==
  - Creamy Potato Pie -- Retail Value ${potato_pie_value}:
  - \- 1 Potato
  - \- 1 Dough
  - \- 1 Cream
  -> continue_to_holding
  
== extra_spicy_quesadilla_recipe ==
  - Extra Spicy Quesadilla -- Retail Value ${extra_spicy_quesadilla_value}:
  - \- 1 Quesadilla
  - \- 1 Firey Pepper
  -> continue_to_holding

== sugar_recipe ==
  - Sugar -- No Retail Value Raw
  - \- 1 Sugar Flies
  -> continue_to_holding

== dough_recipe ==
  - Dough -- No Retail Value Raw
  - \- 1 Wheat
  -> continue_to_holding
  
== cheesey_broccoli_recipe ==
  - Cheesey Broccoli -- Retail Value ${cheesey_broccoli_value}:
  - \- 1 Cheese
  - \- 1 Broccoli
  -> continue_to_holding 

== pepper_popper_recipe ==
  - Pepper Poppers -- Retail Value ${pepper_popper_value}:
  - \- 1 Cheese
  - \- 1 Oil
  - \- 1 Fiery Pepper
  -> continue_to_holding
-> DONE

// using the cooker 
== cooker ==
    - You fire up the ol' cooker. Time to make some food!
    + {inventory_has("berries") && inventory_has("cream")} Berries & Cream (Retail Value: ${berries_and_cream_value})
        -> make_berries_and_cream
    + {inventory_has("dough") && inventory_has("cream")} Creamy Pie (Retail Value: ${creamy_pie_value})
        -> make_creamy_pie
    + {inventory_has("dough") && inventory_has("cream") && inventory_has("potato")} Creamy Potato Pie (Retail Value: ${potato_pie_value})
        -> make_potato_pie
    + {inventory_has("cheese") && inventory_has("broccoli")} Cheesey Broccoli (Retail Value: ${cheesey_broccoli_value})
        -> make_cheesey_broccoli
    + {inventory_has("pepper") && inventory_has("broccoli")} Spicy Broccoli (Retail Value: ${spicy_broccoli_value})
        -> make_spicy_broccoli
    + {inventory_has("gentleBird") && inventory_has("chips")} Gentle Thigh and Chips (Retail Value: ${gentle_thigh_and_chips_value})
        -> make_gentle_thigh_and_chips
    + {inventory_has("cream") && inventory_has("dough") && inventory_has("gentleBird")} Creamy Gentlebird Pie (Retail Value: ${creamy_gentlebird_pie_value})
        -> make_creamy_gentlebird_pie
    + {inventory_has("wheat")} Dough (No Retail Value Raw)
        ->make_dough
    + {inventory_has("dough") && inventory_has("cream") && inventory_has("berries")} Creamy Berry Pie (Retail Value: ${berry_pie_value})
        -> make_berry_pie
    + {inventory_has("dough") && inventory_has("cheese") && inventory_has("guacamole")} Quesadilla (Retail Value: ${quesadilla_value})
        -> make_quesadilla
    + {inventory_has("fish") && inventory_has("dough") && inventory_has("cheese")} Fish Taco (Retail Value: ${fish_taco_value})
        -> make_fish_taco
    + {inventory_has("fish") && inventory_has("chips")} Fish and Chips (Retail Value: ${fish_and_chips_value})
        -> make_fish_and_chips
    + {inventory_has("oil") &&  inventory_has("potato")} Chips (Retail Value: ${chips_value})
        -> make_chips
    + {inventory_has("sugar") &&  inventory_has("potato")} Sweet Potato (No Retail Value Raw)
        -> make_sweet_potato
    + {inventory_has("oil") &&  inventory_has("sweetPotato")} Sweet Potato Fries (Retail Value: ${sweet_potato_fries_value})
        -> make_sweet_potato_fries
    + {inventory_has("dough") && inventory_has("cream") && inventory_has("fish")} Creamy Fish Pie (Retail Value: ${fish_pie_value})
        -> make_fish_pie
    + {inventory_has("dough") && inventory_has("cream") && inventory_has("sugar")} Cake (Retail Value: ${cake_value})
        -> make_cake
    + {inventory_has("cream") && inventory_has("potato")} Mashed Potatoes (Retail Value: ${mashed_potatoes_value})
        -> make_mashed_potatoes
    + {inventory_has("dough")} Bread (Retail Value: ${bread_value})
        -> make_bread
    + {inventory_has("sugarFlies")} Sugar (No Retail Value Raw)
        -> make_sugar
    + {inventory_has("pepper") && inventory_has("quesadilla")} Extra Spicy Quesadilla (Retail Value: ${extra_spicy_quesadilla_value})
        -> make_extra_spicy_quesadilla
    + {inventory_has("pepper") && inventory_has("oil")} Pepper Popper (Retail Value: ${pepper_popper_value})
        -> make_pepper_popper
    
    + Don't cook anything right now
        -> holding_knot

== make_berries_and_cream ==
    - You make some delicious Berries and Cream! # take_berries # take_cream # give_berriesAndCream
    -> continue_to_holding

== make_creamy_pie ==
    - You make some delicious Creamy Pie! # take_cream #take_dough # give_creamyPie
    -> continue_to_holding

== make_potato_pie ==
    - You make some delicious Creamy Potato Pie! # take_potato # take_cream # take_dough # give_potatoPie
    -> continue_to_holding 

== make_cheesey_broccoli ==
    - You make some delicious Cheesey Broccoli ! # take_broccoli # take_cheese # give_cheeseyBroccoli
    -> continue_to_holding 

== make_spicy_broccoli ==
    - You make some delicious Spicy Broccoli! # take_broccoli #take_pepper #give_spicyBroccoli
    -> continue_to_holding 
    
== make_gentle_thigh_and_chips ==
    - You make some delicious Gentle Thigh and Chips! # take_gentleBird # take_chips # give_gentleThighAndChips
    -> continue_to_holding 

== make_creamy_gentlebird_pie ==
    - You make some delicious Creamy Gentlebird Pie! # take_gentleBird # take_cream # take_dough # give_creamyGentlebirdPie
    -> continue_to_holding 

== make_dough ==
    - You make some Dough! # take_wheat # give_dough
    -> continue_to_holding 

== make_berry_pie ==
    - You make some delicious Creamy Berry Pie! # take_berries # take_dough # take_cream # give_berryPie
    -> continue_to_holding 

== make_quesadilla ==
    - You make a delicious Quesadilla! # take_cheese # take_dough # take_guacamole # give_quesadilla
    -> continue_to_holding 

== make_fish_taco ==
    - You make a delicious Fish Taco ! # take_fish # take_cheese # take_dough # give_fishTaco
    -> continue_to_holding 

== make_fish_and_chips ==
    - You make some delicious Fish and Chips! # take_fish # take_chips # give_fishAndChips
    -> continue_to_holding 

== make_chips ==
    - You make some delicious Chips! # take_potato # take_oil # give_chips
    -> continue_to_holding 

== make_sweet_potato_fries ==
    - You make some delicious Sweet Potato Fries! # take_sweetPotato # take_oil # give_sweetPotatoFries
    -> continue_to_holding 
    
== make_sweet_potato ==
    - You make a delicious Sweet Potato! # take_sugar # take_potato # give_sweetPotato
    -> continue_to_holding 
    
== make_fish_pie ==
    - You make some delicious Creamy Fish Pie! # take_fish # take_cream #take_dough # give_fishPie
    -> continue_to_holding 

== make_cake ==
    - You make some delicious Cake! # take_dough # take_sugar # take_cream # give_cake
    -> continue_to_holding 

== make_mashed_potatoes ==
    - You make some delicious Mashed Potatoes! # take_potato # take_cream # give_mashedPotatoes
    -> continue_to_holding 

== make_bread ==
    - You make some delicious Bread! # take_dough # give_bread
    -> continue_to_holding 

== make_sugar ==
    - You make some Sugar! # take_sugarFlies # give_sugar
    -> continue_to_holding 

== make_extra_spicy_quesadilla ==
    - You make a delicious Extra Spicy Quesadilla! # take_quesadilla # take_pepper # give_extraSpicyQuesadilla
    -> continue_to_holding 

== make_pepper_popper ==
    - You make a delicious Pepper Popper! # take_pepper # take_cheese # take_oil # give_pepperPopper
    -> continue_to_holding 