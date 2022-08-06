#pragma once
#include "ComponentsCommon.h"
#include "gtx/matrix_decompose.hpp"

namespace blu::Components
{
	class TransformComponent
	{
	public:
		glm::mat4 Transform;

		TransformComponent() = default;
		TransformComponent(const TransformComponent&) = default;
		TransformComponent(const glm::mat4& in_transform) : Transform{ in_transform } {};

		glm::vec3 GetPosition() {
			glm::vec3 V3padding;
			glm::quat Qpadding;
			glm::vec3 translation;
			glm::vec4 V4perspective;
			glm::decompose(Transform, V3padding, Qpadding, translation, V3padding, V4perspective);
			return translation;
		};

		glm::vec3 GetRotation() {};
		glm::vec3 GetScale() {};
	};
}