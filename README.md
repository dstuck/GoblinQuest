My second unity project to create a beat em up about a weak little goblin

Using Unity 2020.1.4f1

Setting Up License
==================
- Move activation.yml to `.github/workflows`
- Push code to trigger action
- Upload it at license.unity3d.com to receive your `.ulf` file
- Add the contents to a secret named UNITY_LICENSE in Settings > Secrets
- Remove the action from workflows

Local Testing
=============
From the Build directory, start a server: `python -m http.server --cgi 8360`
Then access the server via: `http://localhost:8360/index.html`

Release
=======
`butler push Builds/GoblinQuest.zip drawacard/GoblinQuest/:webgl-beta`

Acknowledgments
===============
Thanks to @Calciumtrice for the sprites https://opengameart.org/content/animated-goblins
