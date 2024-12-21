
<h1 align="center">Unity Conditional Events By David Westberg</h1>

## Overview
Like UnityEvents but a lot more customizable and scriptable.

<img src="https://i.postimg.cc/Mpf4vM0n/image.png" width="75%" height="75%" alt="Image of Conditional Event"/>

## Instructions
**Requirements** (Should work in other versions and render pipelines)
<ul>
<li>Unity 2023.2.20f1 (Built-in)</li>
</ul>

**General Setup**

<ol>
  <li>Download and copy the <code>Core</code>, <code>Extras(Optional)</code> and <code>_Demo(Optional)</code> folders into an empty folder inside your <code>Assets</code> directory</li>
  <li>Add a serialized <code>zombCondEvents.ConditionalEvent</code> variable to any of your scripts to create a new Conditional Event</li>
  <li>In your script, call Init(), EditorTick() and WillDestroy() on your zombCondEvents.ConditionalEvent</li>
  <li>Create your own Events/Conditions by creating a script that inherit from the <code>Condition/Event</code> class (In zombCondEvents namespace)</li>
  <li>Add your Conditions/Events as components to any gameobject and make sure they are assigned in your zombCondEvents.ConditionalEvent inspector</li>
  <li>Call UpdateConditionalEvents on your zombCondEvents.ConditionalEvent to trigger the assigned Events based on the Conditions</li>
  <li>See <code>_Demo/ExampleConditionalEvent.cs</code> for code example</li>
</ol>

## Documentation
Most functions are documented and all parameters visible in the Unity inspector have tooltips

See `_Demo/ExampleConditionalEvent.cs` for ConditionalEvent code example

See `Extras/Cond_maxTriggerCount.cs` for Condition code example

See `Extras/Event_unityEvents.cs` for Event code example

## License
UnityConditionalEvents © 2024 by David Westberg is licensed under MIT - See the `LICENSE` file for more details.

