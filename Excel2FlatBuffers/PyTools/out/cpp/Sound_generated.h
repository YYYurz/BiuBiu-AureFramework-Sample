// automatically generated by the FlatBuffers compiler, do not modify


#ifndef FLATBUFFERS_GENERATED_SOUND_GAMECONFIG_H_
#define FLATBUFFERS_GENERATED_SOUND_GAMECONFIG_H_

#include "flatbuffers/flatbuffers.h"

#include "FP_generated.h"

namespace GameConfig {

struct Sound;

struct SoundList;

struct Sound FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum FlatBuffersVTableOffset FLATBUFFERS_VTABLE_UNDERLYING_TYPE {
    VT_ID = 4,
    VT_GROUPNAME = 6,
    VT_LOOP = 8,
    VT_VOLUME = 10,
    VT_ASSETPATH = 12
  };
  uint32_t Id() const {
    return GetField<uint32_t>(VT_ID, 0);
  }
  const flatbuffers::String *GroupName() const {
    return GetPointer<const flatbuffers::String *>(VT_GROUPNAME);
  }
  uint32_t Loop() const {
    return GetField<uint32_t>(VT_LOOP, 0);
  }
  float Volume() const {
    return GetField<float>(VT_VOLUME, 0.0f);
  }
  const flatbuffers::String *AssetPath() const {
    return GetPointer<const flatbuffers::String *>(VT_ASSETPATH);
  }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<uint32_t>(verifier, VT_ID) &&
           VerifyOffset(verifier, VT_GROUPNAME) &&
           verifier.VerifyString(GroupName()) &&
           VerifyField<uint32_t>(verifier, VT_LOOP) &&
           VerifyField<float>(verifier, VT_VOLUME) &&
           VerifyOffset(verifier, VT_ASSETPATH) &&
           verifier.VerifyString(AssetPath()) &&
           verifier.EndTable();
  }
};

struct SoundBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_Id(uint32_t Id) {
    fbb_.AddElement<uint32_t>(Sound::VT_ID, Id, 0);
  }
  void add_GroupName(flatbuffers::Offset<flatbuffers::String> GroupName) {
    fbb_.AddOffset(Sound::VT_GROUPNAME, GroupName);
  }
  void add_Loop(uint32_t Loop) {
    fbb_.AddElement<uint32_t>(Sound::VT_LOOP, Loop, 0);
  }
  void add_Volume(float Volume) {
    fbb_.AddElement<float>(Sound::VT_VOLUME, Volume, 0.0f);
  }
  void add_AssetPath(flatbuffers::Offset<flatbuffers::String> AssetPath) {
    fbb_.AddOffset(Sound::VT_ASSETPATH, AssetPath);
  }
  explicit SoundBuilder(flatbuffers::FlatBufferBuilder &_fbb)
        : fbb_(_fbb) {
    start_ = fbb_.StartTable();
  }
  SoundBuilder &operator=(const SoundBuilder &);
  flatbuffers::Offset<Sound> Finish() {
    const auto end = fbb_.EndTable(start_);
    auto o = flatbuffers::Offset<Sound>(end);
    return o;
  }
};

inline flatbuffers::Offset<Sound> CreateSound(
    flatbuffers::FlatBufferBuilder &_fbb,
    uint32_t Id = 0,
    flatbuffers::Offset<flatbuffers::String> GroupName = 0,
    uint32_t Loop = 0,
    float Volume = 0.0f,
    flatbuffers::Offset<flatbuffers::String> AssetPath = 0) {
  SoundBuilder builder_(_fbb);
  builder_.add_AssetPath(AssetPath);
  builder_.add_Volume(Volume);
  builder_.add_Loop(Loop);
  builder_.add_GroupName(GroupName);
  builder_.add_Id(Id);
  return builder_.Finish();
}

inline flatbuffers::Offset<Sound> CreateSoundDirect(
    flatbuffers::FlatBufferBuilder &_fbb,
    uint32_t Id = 0,
    const char *GroupName = nullptr,
    uint32_t Loop = 0,
    float Volume = 0.0f,
    const char *AssetPath = nullptr) {
  auto GroupName__ = GroupName ? _fbb.CreateString(GroupName) : 0;
  auto AssetPath__ = AssetPath ? _fbb.CreateString(AssetPath) : 0;
  return GameConfig::CreateSound(
      _fbb,
      Id,
      GroupName__,
      Loop,
      Volume,
      AssetPath__);
}

struct SoundList FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum FlatBuffersVTableOffset FLATBUFFERS_VTABLE_UNDERLYING_TYPE {
    VT_DATA = 4
  };
  const flatbuffers::Vector<flatbuffers::Offset<Sound>> *data() const {
    return GetPointer<const flatbuffers::Vector<flatbuffers::Offset<Sound>> *>(VT_DATA);
  }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyOffset(verifier, VT_DATA) &&
           verifier.VerifyVector(data()) &&
           verifier.VerifyVectorOfTables(data()) &&
           verifier.EndTable();
  }
};

struct SoundListBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_data(flatbuffers::Offset<flatbuffers::Vector<flatbuffers::Offset<Sound>>> data) {
    fbb_.AddOffset(SoundList::VT_DATA, data);
  }
  explicit SoundListBuilder(flatbuffers::FlatBufferBuilder &_fbb)
        : fbb_(_fbb) {
    start_ = fbb_.StartTable();
  }
  SoundListBuilder &operator=(const SoundListBuilder &);
  flatbuffers::Offset<SoundList> Finish() {
    const auto end = fbb_.EndTable(start_);
    auto o = flatbuffers::Offset<SoundList>(end);
    return o;
  }
};

inline flatbuffers::Offset<SoundList> CreateSoundList(
    flatbuffers::FlatBufferBuilder &_fbb,
    flatbuffers::Offset<flatbuffers::Vector<flatbuffers::Offset<Sound>>> data = 0) {
  SoundListBuilder builder_(_fbb);
  builder_.add_data(data);
  return builder_.Finish();
}

inline flatbuffers::Offset<SoundList> CreateSoundListDirect(
    flatbuffers::FlatBufferBuilder &_fbb,
    const std::vector<flatbuffers::Offset<Sound>> *data = nullptr) {
  auto data__ = data ? _fbb.CreateVector<flatbuffers::Offset<Sound>>(*data) : 0;
  return GameConfig::CreateSoundList(
      _fbb,
      data__);
}

}  // namespace GameConfig

#endif  // FLATBUFFERS_GENERATED_SOUND_GAMECONFIG_H_
