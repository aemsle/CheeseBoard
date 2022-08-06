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

		glm::mat4 CreateMatrix()
		{
			glm::mat4 transformation{ 1.0f };

			transformation *= glm::vec4{ position[0],position[1] ,position[2], 0 };
			transformation *= glm::vec4{ rotation[0],rotation[1] ,rotation[2], 0 };
			transformation *= glm::vec4{ scale[0],scale[1] ,scale[2], 1 };

			return transformation;
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
	GameObject* entitiy = new GameObject(in_descriptor->transformDesc.CreateMatrix());
	return entitiy->GetID();
}

EDITOR_INTERFACE
void RemoveGameObject(uint32_t in_id)
{
	GameObject* entitiy = GameObject::GetGameObject(in_id);
	assert(entitiy != nullptr);
	delete entitiy;
	entitiy = nullptr;
	GameObject::GetRegistry();
}

EDITOR_INTERFACE
void Shutdown()
{
	std::vector<GameObject*> activeGameObjects;
	entt::registry* registryRef = GameObject::GetRegistry();
	//get remaining gameobjects
	if (registryRef != nullptr)
	{
		registryRef->each([&activeGameObjects](const auto entity) {activeGameObjects.push_back(GameObject::GetGameObject(entity)); }
		);

		for (GameObject* go : activeGameObjects) //delete all gameobjects
		{
			delete go;
			go = nullptr;
		}

		delete registryRef;//delete registry
		registryRef = nullptr;
	}
}