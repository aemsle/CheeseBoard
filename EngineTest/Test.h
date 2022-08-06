#pragma once

class Test
{
public:
	virtual bool init() = 0;
	virtual void run() = 0;
	virtual void cleanup() = 0;
};