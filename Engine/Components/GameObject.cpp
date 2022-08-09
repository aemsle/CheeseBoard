#include "GameObject.h"

namespace blu
{
	game_object::GameObject::GameObject()
	{
		if (m_registry == nullptr)
		{
			m_registry = new entt::registry();
		}

		m_entitiy = m_registry->create();

		m_registry->emplace<Components::TransformComponent>(m_entitiy);

		m_gameObjects.insert({ m_entitiy, this });
	}

	game_object::GameObject::GameObject(glm::vec3 in_position, glm::quat in_rotation, glm::vec3 in_scale)
	{
		if (m_registry == nullptr)
		{
			m_registry = new entt::registry();
		}

		m_entitiy = m_registry->create();

		m_registry->emplace<Components::TransformComponent>(m_entitiy, in_position, in_rotation, in_scale);

		m_gameObjects.insert({ m_entitiy, this });
	}

	game_object::GameObject::GameObject(const glm::mat4 in_transform)
	{
		if (m_registry == nullptr)
		{
			m_registry = new entt::registry();
		}

		m_entitiy = m_registry->create();

		m_registry->emplace<Components::TransformComponent>(m_entitiy, in_transform);

		m_gameObjects.insert({ m_entitiy, this });
	}

	game_object::GameObject::GameObject(entt::entity in_entity)
	{
		m_entitiy = in_entity;
	}
	entt::registry* game_object::GameObject::m_registry;

	std::unordered_map<entt::entity, game_object::GameObject*> game_object::GameObject::m_gameObjects;
}