#ifndef EDITOR_INTERFACE
#define EDITOR_INTERFACE extern "C" __declspec(dllexport)
#endif // !EDITOR_INTERFACE

#include "..\Engine\Components\GameObject.h"

// glm
#include "gtc/matrix_transform.hpp"
#include "gtx/transform.hpp"

using namespace blu;

namespace
{
	struct TransformDescriptor
	{
		float position[3];
		float rotation[3];
		float scale[3];

		glm::vec3 CreatePosition()
		{
			glm::vec3 out_position{ position[0] ,position[1] ,position[2] };
			return out_position;
		}
		glm::quat CreateRotation()
		{
			glm::quat out_rotation{ glm::vec3{rotation[0] ,rotation[1] ,rotation[2]} };
			return out_rotation;
		}
		glm::vec3 CreateScale()
		{
			glm::vec3 out_scale{ scale[0] ,scale[1] ,scale[2] };
			return out_scale;
		}
	};

	struct GameObjectDescriptor
	{
		TransformDescriptor transformDesc;
	};
}

EDITOR_INTERFACE
entt::entity CreateGameObject(GameObjectDescriptor* in_descriptor)
{
	game_object::GameObject* entitiy = new game_object::GameObject(
		in_descriptor->transformDesc.CreatePosition(),
		in_descriptor->transformDesc.CreateRotation(),
		in_descriptor->transformDesc.CreateScale()
	);
	return entitiy->GetID();
}

EDITOR_INTERFACE
void RemoveGameObject(uint32_t in_id)
{
	game_object::GameObject* entitiy = game_object::GameObject::GetGameObject(in_id);
	assert(entitiy != nullptr);
	delete entitiy;
	entitiy = nullptr;
	game_object::GameObject::GetRegistry();
}

EDITOR_INTERFACE
void Shutdown()
{
	std::vector<game_object::GameObject*> activeGameObjects;
	entt::registry* registryRef = game_object::GameObject::GetRegistry();
	//get remaining gameobjects
	if (registryRef != nullptr)
	{
		registryRef->each([&activeGameObjects](const auto entity) {activeGameObjects.push_back(game_object::GameObject::GetGameObject(entity)); }
		);

		for (game_object::GameObject* go : activeGameObjects) //delete all gameobjects
		{
			delete go;
			go = nullptr;
		}

		delete registryRef;//delete registry
		registryRef = nullptr;
	}
}