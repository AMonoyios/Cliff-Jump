# Cliff-Jump

## Introduction
Single MonoBehavior challenge

### Logs
- Spawn objects to fill width of screen regardless the size/scale.
- Can spawn 2D and 3D assets via settings scriptable object.
- Used a tilted orthographic camera. (*1)
- Implement a Promise system.
- Convert Pool to separate class.
- All assets moved to pool.
- Developed Repo scripts for faster string accessability.
- Scroll scene and spawn new tiles. (IUpdatable)
- Spawn terrain in different heights.
- Implement a collision system. (*2)
- Scale player to 20% of screen dynamically. (*4)
- Implement player "walk" functionality.
- Add player ability jump & double jump.
- Implement a game-over trigger. (*3)
- Bug fixes and final polishings
- Add comments and gameplay testing/overview

### Bugs
- Restart functionality is not working
- Collision is not very accurate sometimes

---

*1 - Placment and/or Rotation doesn't matter because generation is dynamic. <br>
*2 - Without using build-in colliders/rigidbodies. <br>
*3 - When touching side of tiles or bottom of screen. <br>
*4 - Takes the landscape scale and depending on that player dynamically sets scale to 20% of it.
