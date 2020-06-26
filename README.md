### Turrets controller (working name)
**Welcome to the turret controller. Where you as post war AI doing different task to survive.**


### Action System overview

This system tries to solve problem of letting player or AI queuing different actions for units and latter on executing those actions in one non interactive cycle/turn.

- **Unit** - a single unit that can queue actions
- **Action** - definies data requier to execute ``QueuedAction``
- **ActionController** - is responsible for executing ``Action``
- **ActionTarget** - containe information about target location(Vector2) or target(``Unit``) used by ``ActionController``
- **QueuedAction** - contains information necessary to start ``Action``
- **ActionView** - defines how target selection preview for action appears
- **UnitQueueExecutor** - is responsible for executing ``QueuedAction`` for queued ``Units``. The ``units`` order in queue depends on their speed. Every ``QueuedAction`` for current ``Unit`` need to be finished before proceeding to next ``Unit`` in the queue. ``Reaction`` can happend between every ``QueuedAction``. You can think about ``Reaction`` as events, that happend in result of ``QueuedAction``, for example explosion of barrel or dead of enemy. 
