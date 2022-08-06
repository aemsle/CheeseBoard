#include "GameObject.h"

namespace blu
{
	blu::GameObject::GameObject()
	{
		if (m_registry == nullptr)
		{
			m_registry = new entt::registry();
		}

		m_entitiy = m_registry->create();

		m_registry->emplace<Components::TransformComponent>(m_entitiy);

		m_gameObjects.insert({ m_entitiy, this });
	}

	GameObject::GameObject(glm::mat4 in_transform)
	{
		if (m_registry == nullptr)
		{
			m_registry = new entt::registry();
		}

		m_entitiy = m_registry->create();

		m_registry->emplace<Components::TransformComponent>(m_entitiy, in_transform);

		m_gameObjects.insert({ m_entitiy, this });
	}

	entt::registry* blu::GameObject::m_registry;

	std::unordered_map<entt::entity, GameObject*> blu::GameObject::m_gameObjects;
}