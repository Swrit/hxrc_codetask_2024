# Playable build

[itch.io link](https://symbolswriter.itch.io/colorswitch) (Windows and WebGL)

Password: metropolia

# Extra feature suggestions
I think the game's conceptual simplicity is a big part of its appeal, and adding too much may spoil that. Nonetheless, there are some features that can be added.

## High score record
This is fairly simple to implement by just saving the player's best result (highest star count) in PlayerPrefs and displaying it on the game start/game over screens, with a notification when player beats the current record.

It might also be possible to implement an online leaderboard, though that would necessitate having a server to keep the track of the high scores and players creating some sort of account (or using an existing one on Steam/Google Play).

## Pause/Save
Pause button would be great to be able to take a break from the game without losing progress. I would add a button (probably in the upper right color, opposite of the star counter) that when pressed during active game sets Time.timeScale to 1 or Mathf.Epsilon, depending on if the game is currently paused or not.

On a similar note, it would be nice to be able to close the game without losing progress. This would require saving a recording of the current state of the stage (active objects, their positions, rotation, colors, velocity) so the stage can be reconstructed in the same state when the game is next opened. The recording can be saved as JSON file, for example, or converted to string as saved in PlayerPrefs. The saving would be done either when the player closes the game (though this would require a menu/exit button) or at certain checkpoints between obstacles.

## Extra lives/Casual mode
Perhaps a rare pickup item can give player an "extra life". As long as player has spare lives, instead of dying the ball (and camera)would be teleported back to the last safe position (e.g., top edge of the last stage segment below the player).
This feature can be delegated to some sort of "casual mode" - either as the described pickup item idea, or with unlimited lives.

## Autoscroll mode
Another alternative game mode idea is to have the camera constantly move up on its own, independent of the player, with the challenge being to stay within its bounds, neither falling behind or outrunning it.
This would require the stage to be downscaled, so the player can see more of it and have some flexibility regarding when to move and when (and where) to wait.

## Colorblind mode
Since the game relies on colors so much, it might be a good idea to add a colorblind mode. One way to implement that would be to overlay simple and easily recognizable patterns on the colors (like stripes or checkerboard). This can be achieved with custom materials and shaders.

## Object pooling
On a more technical note, instead of simply instantiating and destroying game objects, I would add object pooling. I would implement it in a similar way as I've done for some of my previous projects - 2 dictionaries holding lists of active and inactive instances of prefab clones, where prefab's instance ID is used as a key ([Example](https://github.com/Swrit/Eden7Exodus/blob/main/Eden7Exodus/Scripts/Managers/ObjectPoolManager.cs)).
