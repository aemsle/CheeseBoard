#pragma once
#include "ComponentsCommon.h"
#include "Transform.h"

namespace blu
{
	namespace game_object
	{
		class GameObject
		{
		public:
			GameObject();
			GameObject(glm::mat4);
			GameObject(glm::vec3 in_position, glm::quat in_rotation, glm::vec3 in_scale);
			GameObject(entt::entity);
			GameObject(const GameObject&) = delete;
			GameObject& operator=(const GameObject&) = delete;

			virtual ~GameObject() {
				// #TODO: log error
				assert(m_registry->valid(m_entitiy));
				if (m_registry->valid(m_entitiy))
				{
					m_registry->destroy(m_entitiy);
				}
				else
				{
				}
				if (m_gameObjects.contains(m_entitiy))
				{
					m_gameObjects.erase(m_entitiy);
				}
			}

			entt::entity GetID() { return m_entitiy; }
			bool IsValid() { return m_registry->valid(m_entitiy); }
			static entt::registry* GetRegistry() { return m_registry; }
			static GameObject* GetGameObject(uint32_t in_ID) { return GetGameObject((entt::entity)in_ID); }
			static GameObject* GetGameObject(entt::entity in_ID) {
				// #TODO: log error
				assert(m_gameObjects.contains(in_ID));
				return m_gameObjects.at(in_ID);
			}

		protected:
			static entt::registry* m_registry;
			entt::entity m_entitiy;
			static std::unordered_map<entt::entity, GameObject*> m_gameObjects;
		};
	} // game_object

	namespace object_script
	{
		class ObjectScript : public game_object::GameObject
		{
		public:
			virtual void Start() {}
			virtual void Update(float) {}
		protected:
			ObjectScript(game_object::GameObject in_gameObject)
				: game_object::GameObject(in_gameObject.GetID()) {}
		};

		namespace detail {
			using scriptPtr = std::unique_ptr<ObjectScript>;
			using scriptCreator = scriptPtr(*)(game_object::GameObject);

			template<class script_class>
			scriptPtr CreateScript(game_object::GameObject in_gameObject)
			{
				assert(in_gameObject.IsValid());
				return std::make_unique<script_class>(in_gameObject);
			}
		} // detail
	} // object script
} // blu
