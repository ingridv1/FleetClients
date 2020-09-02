# DocFX help

## Hosting on Localhost

Navigate to ```FleetClients\docfx_project``` and run ```docfx docfx.json``` This will build the documentation and copy it to the ```FleetClients\docs``` folder (where GitHub will look).

Now run ```FleetClients\docfx_project\docfx serve ..\docs``` and the site will be served on ```http://localhost:8080```.

## Editing the Home Page:

The home page lives at ```docfx_project\index.md```

## Adding / Editing Articles

Navigate to: ```docfx_project\articles```. Create or edit a new ```.md``` file your content, then edit ```toc.yml``` to add articles.

## Changing the Template

Navigate to ```docfx_project\templates\guidanceAutomation```
