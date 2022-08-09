#pragma once
#include "ComponentsCommon.h"
#include "gtx/matrix_decompose.hpp"

namespace blu::Components
{
	class TransformComponent
	{
	public:
		glm::vec3 Position{};
		glm::quat Rotation{};
		glm::vec3 Scale{ 1.f };

		TransformComponent() = default;
		TransformComponent(glm::vec3& in_position, const glm::quat& in_rotation, const glm::vec3& in_scale) :
			Position{ in_position }, Rotation{ in_rotation }, Scale{ in_scale } {}
		TransformComponent(const glm::mat4& in_transform)
		{
			glm::decompose(in_transform, Scale, Rotation, Position, skew, perspective);
			Rotation = glm::conjugate(Rotation);
		};

		TransformComponent(const TransformComponent&) = default;

	private:
		glm::vec3 skew{ 0.f };
		glm::vec4 perspective{ 0.f };
	};
}