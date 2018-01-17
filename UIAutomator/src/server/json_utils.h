/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#ifndef __JSON_UTILS_H_
#define __JSON_UTILS_H_

#include <string>
#include <vector>
#include <memory>
#include <mutex>
#include <map>
#include <condition_variable>
#include <Ecore_Con.h>

using namespace std;

class JsonUtils
{
public:
    JsonUtils();
    ~JsonUtils();

    static string GetCommand(char* data);
    static string GetAction(char* data);
    static string FindReply(string elementId);
    static string ActionReply(bool result);
    static string ActionReply(string value);
    static string ActionReply(char* value);
    static string ReplyAxis(string key1, int value1, string key2, int value2 );
    static string GetStringParam(char* data, string key);
    static int GetIntParam(char* data, string key);
private:
};

#endif /* __JSON_UTILS_H_ */