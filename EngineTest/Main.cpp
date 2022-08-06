#define RUN_ESC_TEST 1
#if RUN_ESC_TEST
#include "TestECS.h"
#else
#error At least one test required to be enabled
#endif

int main()
{
#if _DEBUG
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
#endif

	EngineTest test{};

	if (test.init())
	{
		test.run();
	}
	test.cleanup();

	return 0;
}