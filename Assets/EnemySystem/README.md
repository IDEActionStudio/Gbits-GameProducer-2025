Assets/
└── EnemySystem/
├── Core/
│   ├── AI/
│   │   ├── BehaviorTree/               ✨ 行为树子系统
│   │   │   ├── BT_ActionNodes.cs       # 攻击/移动等行为
│   │   │   ├── BT_CompositeNodes.cs    # 顺序/选择等组合逻辑
│   │   │   └── BT_ConditionNodes.cs    # 距离/血量等条件判断
│   │   ├── Blackboard.cs               # AI数据黑板系统
│   │   └── EnemyBrain.cs               # AI主控制器
│   │
│   ├── Events/
│   │   └── EnemyEventSystem.cs         # 敌人专属事件总线
│   │
│   ├── Interfaces/
│   │   ├── IDamageable.cs              # 跨系统伤害接口[3](@ref)
│   │   └── IEnemyModule.cs             # 模块化基础契约
│   │
│   ├── Managers/
│   │   ├── EnemyManager.cs             # 敌人生成调度
│   │   └── EnemyPoolCoordinator.cs     ✨ 对象池协调器[6](@ref)
│   │
│   └── StateMachine/
│       ├── Factories/
│       │   └── EnemyStateFactory.cs    # 状态工厂模式实现[3](@ref)
│       ├── Interfaces/                ✨ 状态机接口隔离
│       │   └── IEnemyState.cs          # 状态生命周期接口
│       ├── EnemyStateMachine.cs        # 状态机主控制器
│       └── States/
│           ├── AttackState.cs          # 攻击状态（含子状态机）
│           └── ChaseState.cs           # 追击状态（含过渡条件）
│
├── Data/
│   ├── Configs/
│   │   ├── ScriptableObjects/         ✨ 配置驱动设计
│   │   │   ├── AIBehaviorConfig.asset # 行为树配置[1](@ref)
│   │   │   └── EnemyConfig.asset      # 基础属性配置
│   │   └── ConfigLoader.cs            # 配置加载器
│   │
│   ├── Enums/
│   │   ├── AIState.cs                 # AI状态枚举
│   │   └── EnemyType.cs               # 敌人类型枚举
│   │
│   └── Runtime/                      ✨ 运行时数据隔离
│       └── EnemyRuntimeData.cs        # 动态生成数据容器
│
├── Modules/
│   ├── Combat/
│   │   ├── Effects/
│   │   │   ├── VFX/                  ✨ 特效资源隔离
│   │   │   └── ImpactProcessor.cs    # 受击效果处理器
│   │   ├── Interfaces/
│   │   │   └── ICombat.cs            # 战斗行为接口
│   │   ├── Projectiles/
│   │   │   ├── HomingProjectile.cs    # 制导型投射物
│   │   │   └── ProjectileBase.cs      # 投射物基类
│   │   └── CombatSystem.cs            # 攻击逻辑控制器
│   │
│   ├── Movement/
│   │   ├── Animations/
│   │   │   └── LocomotionHandler.cs   # 运动动画状态机
│   │   ├── Interfaces/
│   │   │   └── IMovement.cs           # 移动行为接口
│   │   ├── Pathfinding/
│   │   │   ├── AStarPathfinder.cs     # A*算法实现
│   │   │   └── NavMeshAdapter.cs      ✨ Unity导航适配
│   │   └── MovementController.cs       # 移动主逻辑
│   │
│   └── Perception/
│       ├── Interfaces/
│       │   └── IPerception.cs        # 感知系统接口
│       ├── Memory/
│       │   └── TargetMemory.cs       # 目标记忆系统
│       ├── Sensors/
│       │   ├── SoundSensor.cs         # 声波检测系统
│       │   └── VisionSensor.cs        # 视锥检测系统
│       └── PerceptionManager.cs       # 感知主控制器
│
├── Prefabs/                          ✨ 模块化预制体管理
│   ├── Components/
│   │   ├── Projectiles/              # 可复用投射物
│   │   └── VFX/                      # 特效预制体
│   └── Enemies/
│       ├── MeleeEnemy.prefab         # 近战敌人预制体
│       └── RangedEnemy.prefab        # 远程敌人预制体
│
├── Artwork/                          ✨ 美术资源规范[6](@ref)
│   ├── Materials/                    # 专用材质球
│   ├── Models/                       # FBX模型文件
│   ├── Shaders/                      # 自定义着色器
│   └── Textures/                     # 角色/武器贴图
│
├── ThirdParty/                       ✨ 插件隔离[2](@ref)
│   └── AIPlugins/                    # 第三方AI插件
│
└── Utilities/
├── Debug/
│   ├── AIVisualizer.cs           # 行为树可视化
│   └── StateTracker.cs           # 状态机调试工具
│
├── Editor/                       ✨ 编辑器扩展
│   ├── AIDebugWindow.cs          # AI调试面板
│   └── EnemyConfigEditor.cs       # 配置可视化工具
│
└── Pooling/                      ✨ 对象池系统
├── Interfaces/
│   │   └── IPoolable.cs          # 池化对象契约
└── PoolManager.cs            # 通用池实现