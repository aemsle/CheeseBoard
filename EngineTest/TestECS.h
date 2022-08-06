#pragma once
#include "Test.h"
#include "../Engine/Components/Transform.h"
#include "../Engine/Components/GameObject.h"
#include "entt/entt.hpp"
#include <iostream>
#include <ctime>
#include <stdio.h>

using namespace blu;

class EngineTest : public Test
{
public:
	bool init() override
	{
		srand((uint32_t)time(nullptr));
		return true;
	}

	void run() override
	{
		do {
			for (uint32_t i = 0; i < 10000; i++)
			{
				create_random();
				remove_random();
				num_entities = (uint32_t)entities.size();
			}
			print_results();
		} while (getchar() != 'q');
	}

	void cleanup() override
	{
		for (GameObject* go : entities)
		{
			delete go;
			go = nullptr;
		}
	}
private:
	std::vector<GameObject*> entities;
	uint32_t added{ 0 };
	uint32_t removed{ 0 };
	uint32_t num_entities{ 0 };

	void create_random()
	{
		uint32_t count = rand() % 20;
		if (entities.empty()) count = 1000;

		while (count > 0)
		{
			added++;

			entities.push_back(new GameObject());
			assert(entities.back()->IsValid());
			count--;
		}
	}

	void remove_random()
	{
		uint32_t count = rand() % 20;
		if (entities.size() < 1000) return;

		while (count > 0)
		{
			const uint32_t index{ rand() % entities.size() };
			if (entities[index]->IsValid())
			{
				delete entities[index];
				entities[index] = nullptr;

				entities.erase(entities.begin() + index);
				removed++;
			}
			count--;
		}
	}

	void print_results()
	{
		std::cout << "Entities created: " << added << std::endl;
		std::cout << "Entities deleted: " << removed << std::endl;
		std::cout << "Number of entities: " << num_entities << std::endl;
	}
};